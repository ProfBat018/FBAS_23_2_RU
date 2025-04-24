using ControllerFirst.Contexts;
using ControllerFirst.Data.Models;
using ControllerFirst.DTO.Requests;
using ControllerFirst.DTO.Responses;
using ControllerFirst.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControllerFirst.Services.Classes;

public class RoleService : IRoleService
{
    private readonly AuthContext _context;

    public RoleService(AuthContext context)
    {
        _context = context;
    }

    public Task<RoleResponse> CreateRoleAsync(CreateRoleRequest dto)
    {
        throw new NotImplementedException();
    }

    public Task<RoleResponse> GetRoleAsync(string name)
    {
        throw new NotImplementedException();
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
        var userRole = await _context.UserRoles.FirstOrDefaultAsync(u => u.UserRef == Guid.Parse(id) && u.RoleNameRef == roleName);
        
        if (userRole == null)
        {
            throw new Exception("User role not found");
        }
        
        _context.UserRoles.Remove(userRole);
        
        await _context.SaveChangesAsync();
    }
}