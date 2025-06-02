using System.IdentityModel.Tokens.Jwt;
using ControllerFirst.DTO.Requests;
using ControllerFirst.Services.Interfaces;

namespace ControllerFirst.Middlewares;

public class JwtRefreshMiddleware : IMiddleware
{
    private readonly ITokenService _tokenService;
    private readonly IAuthService _authService;

    public JwtRefreshMiddleware( ITokenService tokenService, IAuthService authService)
    {
        _tokenService = tokenService;
        _authService = authService;
    }
    

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var accessToken = context.Request.Cookies["accessToken"];
        var refreshToken = context.Request.Cookies["refreshToken"];

        if (context.Request.Path == "/api/v1/auth/login")
        {
            await next(context); 
            return;
        }
        
        if (!string.IsNullOrEmpty(accessToken))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(accessToken);

            var exp = token.ValidTo;
            if (exp < DateTime.UtcNow && !string.IsNullOrEmpty(refreshToken))
            {
                var username = await _tokenService.GetNameFromToken(accessToken);
                var result = await _authService.RefreshTokenAsync(new RefreshTokenRequest(username, refreshToken));

                context.Response.Cookies.Append("accessToken", result.accessToken);
                context.Response.Cookies.Append("refreshToken", result.refreshToken);
            }
        }

        await next(context);
    }
}