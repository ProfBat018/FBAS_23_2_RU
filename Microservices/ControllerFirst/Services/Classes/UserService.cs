using ControllerFirst.Contexts;
using ControllerFirst.DTO.Requests;
using ControllerFirst.DTO.Responses;
using ControllerFirst.Hubs;
using ControllerFirst.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace ControllerFirst.Services.Classes;

public class UserService : IUserService
{
    private readonly AuthContext _context;
    private readonly IUserElasticService _elasticService;
    private readonly IHubContext<UserNotificationHub> _hubContext;


    public UserService(AuthContext context, IUserElasticService elasticService, IHubContext<UserNotificationHub> hubContext)
    {
        _context = context;
        _elasticService = elasticService;
        _hubContext = hubContext;
    }

    public async Task<PaginatedResult<UserResponse>> GetUsersAsync(int page = 1, int pageSize = 5)
    {
        var users = await _context.Users
            .Skip((page - 1) * pageSize)
            .Select(u => new UserResponse(u.Id, u.UserName, u.Email, u.IsEmailConfirmed))
            .Take(pageSize).ToListAsync();
        
        var totalUsers = await _context.Users.CountAsync();
        
        return  PaginatedResult<UserResponse>.Success(users, page, pageSize,  totalUsers);
    }

    public async Task<UserInfoResponse> GetUserInfoAsync(Guid Id)
    {
        var user = await _context.Users.FindAsync(Id);

        var userRoles = await _context.UserRoles.Where(ur => ur.UserRef == user.Id)
            .Select(ur => ur.RoleNameRef).ToListAsync();
        
        return new UserInfoResponse(user.UserName, user.Email, user.IsEmailConfirmed, userRoles);

    }

    public async Task<RolesInfoResponse> GetUserRolesAsync(Guid Id)
    {
        var user = await _context.Users.FindAsync(Id);

        var userRoles = await _context.UserRoles.Where(ur => ur.UserRef == user.Id)
            .Select(ur => ur.RoleNameRef).ToListAsync();
        
        return new RolesInfoResponse(Id.ToString(), userRoles);
    }

    public async Task ConfirmEmailAsync(ConfirmFromAdminRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(request.id));
        
        user.IsEmailConfirmed = true;
        
        await _context.SaveChangesAsync();
        
    }
    
    public async Task DeleteUserAsync(Guid id)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == id);
        
        
        _context.UserRoles.RemoveRange(user.UserRoles);
        _context.Users.Remove(user);
        
        await _context.SaveChangesAsync();
        await _elasticService.DeleteUserAsync(user.Id);
        
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", new NotificationMessage("UserDeleted", new { Id = user.Id, Name = user.UserName }));
        
    }
}   