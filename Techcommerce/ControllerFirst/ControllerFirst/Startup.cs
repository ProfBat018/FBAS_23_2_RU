using System.Text;
using ControllerFirst.Contexts;
using ControllerFirst.Data.Mapping;
using ControllerFirst.Data.Validators;
using ControllerFirst.DTO.Requests;
using ControllerFirst.Services.Classes;
using ControllerFirst.Services.Interfaces;
using ControllerFirst.Shared;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<RegisterValidator>();

        services.AddEndpointsApiExplorer();
        services.AddOpenApi();

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.Strict;
            options.HttpOnly = HttpOnlyPolicy.Always;
            options.Secure = CookieSecurePolicy.None;
        });

        services.AddCors(policy =>
        {
            policy.AddPolicy("Default", builder =>
            {
                builder
                    .WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        services.AddTransient<AuthContext>();

        // JWT configuration
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
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.HttpContext.Request.Cookies["accessToken"];
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    },
                    OnForbidden = async context =>
                    {
                        var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
                        var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();

                        var accessToken = context.Request.Cookies["accessToken"];
                        var refreshToken = context.Request.Cookies["refreshToken"];

                        var username = await tokenService.GetNameFromToken(accessToken);

                        var result = await authService.RefreshTokenAsync(new RefreshTokenRequest(username, refreshToken));

                        context.Response.Cookies.Append("accessToken", result.accessToken);
                        context.Response.Cookies.Append("refreshToken", result.refreshToken);

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    }
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy => policy.RequireRole("AppAdmin"));
            options.AddPolicy("UserPolicy", policy => policy.RequireRole("AppUser", "AppAdmin"));
        });

        services.AddAutoMapper(ops => { ops.AddProfile<UserProfile>(); });

        services.AddDbContext<AuthContext>(ops =>
        {
            ops.UseSqlServer(_configuration.GetConnectionString("Default"));
        });

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<GlobalExceptionMiddleware>();
    }

    public void Configure(WebApplication app)
    {
        app.UseCors("Default");

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHttpsRedirection();

        app.MapControllers();
        app.MapOpenApi();
        app.MapScalarApiReference();

        app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}