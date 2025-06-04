using System.Text;
using ControllerFirst.Contexts;
using ControllerFirst.Data.Mapping;
using ControllerFirst.Data.Models;
using ControllerFirst.Data.Validators;
using ControllerFirst.DTO.Requests;
using ControllerFirst.Hubs;
using ControllerFirst.Middlewares;
using ControllerFirst.Services.Classes;
using ControllerFirst.Services.Interfaces;
using ControllerFirst.Shared;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nest;
using Scalar.AspNetCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSignalR();
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<RegisterValidator>();

        services.AddEndpointsApiExplorer();
        services.AddOpenApi();
        

        services.AddCors(ops =>
        {
            ops.AddPolicy("DefaultPolicy", policy =>
            {
                policy.WithOrigins("http://localhost:3000", "http://localhost:5173", "http://127.0.0.1:5500")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.Strict;
            options.HttpOnly = HttpOnlyPolicy.Always;
            options.Secure = CookieSecurePolicy.None;
        });



        services.AddTransient<AuthContext>();

    
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["JWT:Issuer"],
                    ValidAudience = _configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = async (context) =>
                    {
                        var accessToken = context.HttpContext.Request.Cookies["accessToken"];
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                        }
                    }
                };
            });
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy => policy.RequireRole("AppAdmin", "SuperAdmin"));
            options.AddPolicy("UserPolicy", policy => policy.RequireRole("AppUser", "AppAdmin", "SuperAdmin"));
        });

        services.AddAutoMapper(ops =>
        {
            ops.AddProfile<UserProfile>();
            ops.AddProfile<UserCardProfile>();
            ops.AddProfile<UserAddressProfile>();
        });

        services.AddDbContext<AuthContext>(ops =>
        {
            Console.WriteLine($"FIRST: {_configuration["ConnectionStrings:Default"]}");
            ops.UseSqlServer(_configuration["ConnectionStrings:Default"]);
        });

        
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ICardService, CardService>();
        services.AddScoped<IAddressService, AddressService>();
        services.AddScoped<GlobalExceptionMiddleware>();
        services.AddScoped<IElasticReindexService<UserElasticDTO>, UserElasticReindexService>();
        services.AddScoped<IUserElasticService, UserElasticService>();
        services.AddScoped<JwtRefreshMiddleware>();
        
        services.AddSingleton<EncryptionService>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var secretKey = configuration["Encryption:SecretKey"]; // прочитает из appsettings
            return new EncryptionService(secretKey);
        });
        services.AddSingleton<IElasticClient>(sp =>
        {
            var uri = _configuration["ElasticConfiguration:Uri"];
            var apiKey = _configuration["ElasticConfiguration:ApiKey"];
            var defaultIndex = _configuration["ElasticConfiguration:DefaultIndex"];

            Console.WriteLine(uri);
            
            var settings = new ConnectionSettings(new Uri(uri))
                .ApiKeyAuthentication(new(apiKey))
                .DefaultIndex(defaultIndex)
                .EnableDebugMode()
                .PrettyJson();

            return new ElasticClient(settings);
        });

        services.Configure<RabbitMqSettings>(_configuration.GetSection("RabbitMQ"));
        services.AddHostedService<RabbitMqListener>();

    }

    public void Configure(WebApplication app)
    {
        app.UseCors("DefaultPolicy");
        
        app.UseMiddleware<JwtRefreshMiddleware>();
        
        app.UseAuthentication();

        app.UseAuthorization();
        
        app.UseHttpsRedirection();
        
        app.MapControllers();
        
        app.MapHub<UserNotificationHub>("/hubs/notification");
        
        app.MapOpenApi();
        
        app.MapScalarApiReference();

        // using (var scope = app.Services.CreateScope())
        // {
        //     var dbContext = scope.ServiceProvider.GetRequiredService<AuthContext>();
        //     DbInitializer.SeedDatabase(dbContext);
        // }
        
        app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}