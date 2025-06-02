using ControllerFirst.Contexts;
using ControllerFirst.Data.Models;
using ControllerFirst.DTO.Requests;
using ControllerFirst.DTO.Responses;
using ControllerFirst.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ControllerFirst.Services.Classes;

public class RoleService : IRoleService
{
    private readonly AuthContext _context;

    public RoleService(AuthContext context)
    {
        _context = context;
    }

    public async Task<RoleResponse> CreateRoleAsync(CreateRoleRequest dto)
    {
        var role = new Role()
        {
            RoleName = dto.roleName
        };

        _context.Roles.Add(role);

        await _context.SaveChangesAsync();
        
        return new RoleResponse(role.RoleName);
    }

    public Task<RoleResponse> GetRoleAsync(string name)
    {
        var role = _context.Roles.FirstOrDefaultAsync(r => r.RoleName == name);

        if (role == null)
        {
            throw new Exception("Role not found");
        }

        return Task.FromResult(new RoleResponse(role.Result.RoleName));
    }

    public async Task<PaginatedResult<RoleResponse>> GetRolesAsync(int page = 1, int pageSize = 10)
    {
        var roles = await _context.Roles
            .Select(r => new RoleResponse(r.RoleName)).ToListAsync();

        return PaginatedResult<RoleResponse>.Success(roles, 1, 5, roles.Count);
    }


    public async Task MapRoleToUserAsync(string id, string roleName)
    {
        var user = await _context.Users.FindAsync(Guid.Parse(id));

        _context.UserRoles.AddAsync(new UserRole()
        {
            UserRef = user.Id,
            RoleNameRef = roleName
        });

        await _context.SaveChangesAsync();
    }

    public async Task UnMapRoleToUserAsync(string id, string roleName)
    {
        var userRole =
            await _context.UserRoles.FirstOrDefaultAsync(u => u.UserRef == Guid.Parse(id) && u.RoleNameRef == roleName);

        if (userRole == null)
        {
            throw new Exception("User role not found");
        }

        _context.UserRoles.Remove(userRole);

        await _context.SaveChangesAsync();
    }

    public async Task<RoleResponse> UpdateRoleAsync(UpdateRoleRequest request)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == request.oldName);

        if (role == null)
        {
            throw new Exception("Role not found");
        }

        role.RoleName = request.newName;

       await  _context.SaveChangesAsync();
        return new RoleResponse(role.RoleName);
    }

    public async Task<RoleResponse> DeleteRoleAsync(DeleteRoleRequest request)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == request.roleName);
        
        if (role == null)
        {
            throw new Exception("Role not found");
        }
        
        _context.Roles.Remove(role);
        
        await _context.SaveChangesAsync();
        
        return new RoleResponse(role.RoleName);
    }
}