using ControllerFirst.DTO.Requests;
using ControllerFirst.DTO.Responses;
using ControllerFirst.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace ControllerFirst.Controllers;

[Authorize(Policy = "AdminPolicy")]
[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("All/{page}/{pageSize}")]
    public async Task<IActionResult> GetUsersAsync(int page=1, int pageSize=5)
    {
        var users = await _userService.GetUsersAsync(page, pageSize);
        return Ok(users);
    }
    
    [HttpGet("{Id}/Roles")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetUserRolesAsync(string Id)
    {
        var user = await _userService.GetUserRolesAsync(Guid.Parse(Id));
        return Ok(Result<RolesInfoResponse>.Success(user, "User roles retrieved successfully"));
    }
    
    [HttpPost("Email/Confirm")]
    public async Task<IActionResult> ConfirmEmailAsync([FromBody] ConfirmFromAdminRequest request)
    {
        await _userService.ConfirmEmailAsync(request);
        return Ok(Result<bool>.Success(true, "Email confirmed successfully"));
    }
    
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        await _userService.DeleteUserAsync(id);  
        return Ok(Result<bool>.Success(true, "User deleted successfully"));
    }
}