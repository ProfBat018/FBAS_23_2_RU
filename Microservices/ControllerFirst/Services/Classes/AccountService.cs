using System.Net;
using System.Net.Mail;
using System.Text;
using AutoMapper;
using ControllerFirst.Contexts;
using ControllerFirst.Data.Models;
using ControllerFirst.DTO.Requests;
using ControllerFirst.DTO.Responses;
using ControllerFirst.Hubs;
using ControllerFirst.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;

namespace ControllerFirst.Services.Classes;

public class AccountService : IAccountService
{
    private readonly IMapper _mapper;
    private readonly AuthContext _context;
    private readonly IConfiguration _config;
    private readonly ITokenService _tokenService;
    private readonly IUserElasticService _elasticService;
    private readonly Random _random = new();
    private readonly IHubContext<UserNotificationHub> _hubContext;
    private readonly IWebHostEnvironment _env;


    public AccountService(IMapper mapper, AuthContext authContext, IConfiguration config, ITokenService tokenService, IUserElasticService elasticService, IHubContext<UserNotificationHub> hubContext, IWebHostEnvironment env)
    {
        _mapper = mapper;
        _context = authContext;
        _config = config;
        _tokenService = tokenService;
        _elasticService = elasticService;
        _hubContext = hubContext;
        _env = env;
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        
        var user = _mapper.Map<User>(request);

        user.Password = HashPassword(user.Password);

        await _context.Users.AddAsync(user);

        await _context.SaveChangesAsync();

        await _context.UserRoles.AddAsync(new UserRole
        {
            UserRef = user.Id,
            RoleNameRef = "AppUser"
        });

        await _context.SaveChangesAsync();

        await _elasticService.IndexUserAsync(user);
        
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", new NotificationMessage(
            "UserCreated",
            new { Id = user.Id, user.UserName, user.Email }
        ));
    }

    public async Task ConfirmEmailAsync(string username, HttpContext context)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

        SmtpClient client = new SmtpClient()
        {
            Port = 587,
            EnableSsl = true,
            Host = _config["Smtp:Host"],
            Credentials = new NetworkCredential(_config["Smtp:Username"], _config["Smtp:Password"])
        };

        using FileStream fs = new(Path.Combine(_env.WebRootPath, "email.html"), FileMode.Open);
        using StreamReader sr = new(fs);
        
        StringBuilder sb = new(await sr.ReadToEndAsync());

        var link =
            $"{context.Request.Scheme}://{context.Request.Host}/api/v1/Account/Email/Verify?token={await _tokenService.CreateEmailTokenAsync(username)}";

        sb.Replace("{username}", username);
        sb.Replace("{link}", link);

        MailMessage message = new()
        {
            From = new MailAddress(_config["Smtp:Username"]),
            Subject = "Email confirmation",
            Body = sb.ToString(),
            IsBodyHtml = true
        };


        message.To.Add(user.Email);

        client.Send(message);
    }

    public async Task VerifyEmailAsync(string token)
    {
        var name = await _tokenService.GetNameFromToken(token);

        bool res = await _tokenService.ValidateEmailTokenAsync(token);
        
        if (res)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == name);

            user.IsEmailConfirmed = true;

            await _context.SaveChangesAsync();
        }
        else
        {
            throw new Exception("Invalid token");
        }
    }

    public async Task ChangeUserNameAsync(string oldUsername, string newUsername)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == oldUsername);
        
        user.UserName = newUsername;
        
        await _context.SaveChangesAsync();

        await _elasticService.IndexUserAsync(user);
    }
    
    private  string GeneratePassword(int length = 8)
    {
        const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
        const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string digits = "0123456789";
        const string specialChars = "_*$#@!%";
        const string allChars = lowerCase + upperCase + digits + specialChars;

        if (length < 8)
            throw new ArgumentException("Пароль должен быть минимум 8 символов");


        char[] password = new char[length];
        password[0] = lowerCase[_random.Next(lowerCase.Length)];
        password[1] = upperCase[_random.Next(upperCase.Length)];
        password[2] = digits[_random.Next(digits.Length)];
        password[3] = specialChars[_random.Next(specialChars.Length)];


        for (int i = 4; i < length; i++)
        {
            password[i] = allChars[_random.Next(allChars.Length)];
        }


        return new string(password.OrderBy(_ => _random.Next()).ToArray());
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(request.id));

        var newPassword = GeneratePassword();
        
        user.Password = HashPassword(newPassword);
        
        SmtpClient client = new SmtpClient()
        {
            Port = 587,
            EnableSsl = true,
            Host = _config["Smtp:Host"],
            Credentials = new NetworkCredential(_config["Smtp:Username"], _config["Smtp:Password"])
        };
        
        using FileStream fs = new(Path.Combine(_env.WebRootPath, "reset.html"), FileMode.Open);
        using StreamReader sr = new(fs);
        StringBuilder sb = new(await sr.ReadToEndAsync());
        
        sb.Replace("{username}", user.UserName);
        sb.Replace("{newPassword}", newPassword);
        
        
        MailMessage message = new()
        {
            From = new MailAddress(_config["Smtp:Username"]),
            Subject = "Password reset",
            Body = sb.ToString(),
            IsBodyHtml = true
        };
        
        message.To.Add(user.Email);
        
        client.Send(message);
        
        await _context.SaveChangesAsync();
    }

    public async Task ChangeEmailAsync(ChangeEmailRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(request.id));
        
        user.Email = request.newEmail;
        user.IsEmailConfirmed = false;
        
        await _context.SaveChangesAsync();
        
        await _elasticService.IndexUserAsync(user);
    }

    public async Task DeleteAccountAsync(string id)
    {
        var user = await _context.Users.FindAsync(Guid.Parse(id));
        
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        _context.Users.Remove(user);
        
        await _context.SaveChangesAsync();
        
        await _elasticService.DeleteUserAsync(user.Id);
        
    }

    public async Task ChangePasswordAsync(string username, ChangePasswordRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (!Verify(request.oldPassword, user.Password))
        {
            throw new Exception("Invalid password");
        }

        user.Password = HashPassword(request.newPassword);

        await _context.SaveChangesAsync();
    }

    public async Task<UserInfoResponse> GetProfileAsync(string username)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.UserName == username);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        var roles = await _context.UserRoles
            .Where(ur => ur.UserRef == user.Id)
            .Select(ur => ur.RoleNameRef)
            .ToListAsync();
        
        var response = new UserInfoResponse(user.UserName, user.Email, user.IsEmailConfirmed, roles);
        
        return response;
    }
}