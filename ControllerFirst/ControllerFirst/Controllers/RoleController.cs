using ControllerFirst.DTO.Requests;
using ControllerFirst.DTO.Responses;
using ControllerFirst.Services.Classes;
using ControllerFirst.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControllerFirst.Controllers;


[ApiController]
[Authorize(Policy = "AdminPolicy")]
[Route("api/v1/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost("Set")]
    public async Task<IActionResult> SetRole(RoleMapRequest request)
    {
        await _roleService.MapRoleToUserAsync(request.id, request.roleName);
        
        return Ok("Role set");
    }
    
    [HttpPost("Unset")]
    public async Task<IActionResult> UnsetRole(RoleMapRequest request)
    {
        await _roleService.UnMapRoleToUserAsync(request.id, request.roleName);

        if (request.roleName == "SuperAdmin")
        {
            return BadRequest("Cannot unset SuperAdmin role");
        }
        
        return Ok("Role unset");
    }
    
    [HttpGet("All")]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _roleService.GetRolesAsync();
        return Ok(roles);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateRole()
    {
        return Ok("Role created");
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateRole()
    {
        return Ok("Role updated");
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteRole()
    {
        return Ok("Role deleted");
    }
}