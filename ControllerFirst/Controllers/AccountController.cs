using System.Security.Claims;
using ControllerFirst.Data.Validators;
using ControllerFirst.DTO.Requests;
using ControllerFirst.DTO.Responses;
using ControllerFirst.Services.Classes;
using ControllerFirst.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace ControllerFirst.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }


    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        await _accountService.RegisterAsync(request);

        return Ok(new Result<string>(true, request.Username, "Successfully registered"));
    }


    [Authorize(Policy = "UserPolicy")]
    [HttpGet("Email/Verify")]
    public async Task<IActionResult> VerifyEmailAsync([FromQuery] string token)
    {
        await _accountService.VerifyEmailAsync(token);

        return Ok(new Result<string>(true, "Email confirmed", "Email confirmed"));
    }

    [Authorize]
    [HttpPost("Email/Change")]
    public async Task<IActionResult> ChangeEmailAsync([FromBody] ChangeEmailRequest request)
    {
        await _accountService.ChangeEmailAsync(request);

        return Ok(new Result<string>(true, request.newEmail, "Email changed"));
    }

    [Authorize(Policy = "UserPolicy")]
    [HttpPost("Email/Confirm")]
    public async Task<IActionResult> ConfirmEmailAsync()
    {
        var username = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
        
        await _accountService.ConfirmEmailAsync(username, HttpContext);

        return Ok(new Result<string>(true, username, "Email sent"));
    }

    [HttpPost("Password/Reset")]
    public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        await _accountService.ResetPasswordAsync(request);

        return Ok(new Result<string>(true, "Password reset"));
    }

    [HttpPost("Password/Change")]
    public async Task<IActionResult> ChangePasswordAsync()
    {
        throw new NotImplementedException();
    }


    [Authorize(Policy = "UserPolicy")]
    [HttpPost("Username/Change")]
    public async Task<IActionResult> ChangeUsernameAsync([FromBody] ChangeUsernameRequest request)
    {
        var usrClaims = User.Claims.First(c => c.Type == ClaimTypes.Name);

        var username = usrClaims.Value;

        await _accountService.ChangeUserNameAsync(username, request.newUsername);

        return Ok("Username changed");
    }
}