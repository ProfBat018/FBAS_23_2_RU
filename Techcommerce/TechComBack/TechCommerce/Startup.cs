using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using TechCommerce.Data.Contexts;
using TechCommerce.Services.Implementations;
using TechCommerce.Services.Interfaces;

namespace TechCommerce;

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
                        var accessToken = context.Request.Cookies["accessToken"];
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy => policy.RequireRole("AppAdmin"));
            options.AddPolicy("UserPolicy", policy => policy.RequireRole("AppUser", "AppAdmin"));
        });

        services.AddScoped<ICategoryService, CategoryService>();

        services.AddOpenApi();
        services.AddEndpointsApiExplorer();

        services.AddDbContext<TechContext>(ops => ops.UseSqlServer(
            _configuration.GetConnectionString("Default")));
    }


    public void Configure(WebApplication app)
    {
        app.MapOpenApi();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();
        app.MapScalarApiReference();

        app.UseHttpsRedirection();
    }
}