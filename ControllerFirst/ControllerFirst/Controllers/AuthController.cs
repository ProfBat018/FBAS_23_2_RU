using ControllerFirst.Data.Models;
using ControllerFirst.Data.Validators;
using ControllerFirst.DTO.Requests;
using ControllerFirst.DTO.Responses;
using ControllerFirst.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace ControllerFirst.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;

    public AuthController(IAuthService authService, ITokenService tokenService)
    {
        _tokenService = tokenService;
        _authService = authService;
    }

    [HttpGet("Check")]
    public async Task<IActionResult> CheckAuth()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return Ok(new { isAuthenticated = true });
        }
        return Unauthorized(new { isAuthenticated = false });
    }
    
    [HttpPost("Login")]
    public async  Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        
        Response.Cookies.Append("accessToken", response.accessToken);
        Response.Cookies.Append("refreshToken", response.refreshToken);
        
        
        return Ok(new Result<LoginResponse>(true, response, "Successfully logged in"));
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("Me")]
    public async Task<IActionResult> GetUserData()
    {
        return Ok(Result<bool>.Success(true, "You are an admin"));
    }

    [HttpPost("Refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var accessToken = Request.Cookies["accessToken"];
        
        var request = new RefreshTokenRequest(await _tokenService.GetNameFromToken(accessToken), refreshToken);
        
        var newTokens = await _authService.RefreshTokenAsync(request);
        
        Response.Cookies.Append("accessToken", newTokens.accessToken);
        Response.Cookies.Append("refreshToken", newTokens.refreshToken);
        
        return Ok(new Result<RefreshTokenResponse>(true, newTokens, "Successfully refreshed token"));
        
    }

    [HttpPost("Test")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Test()
    {
        return Ok("Test");
    }
    
    [Authorize(Policy = "UserPolicy")]
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {

        var logOutRes = await _authService.LogOutAsync(User);

        Response.Cookies.Delete("accessToken");
        Response.Cookies.Delete("refreshToken");
        
        return Ok(Result<LogOutResponse>.Success(logOutRes));

    }
}
