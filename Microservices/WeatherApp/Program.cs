using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WeatherApp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSingleton<RabbitMqService>();

builder.WebHost.ConfigureKestrel(ops =>
{
    ops.ListenAnyIP(5002);
});


builder.Services.AddAuthentication(options =>
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
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
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
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("AppAdmin", "SuperAdmin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("AppUser", "AppAdmin", "SuperAdmin"));
});

var app = builder.Build();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApi();

// app.UseHttpsRedirection();


app.Run();