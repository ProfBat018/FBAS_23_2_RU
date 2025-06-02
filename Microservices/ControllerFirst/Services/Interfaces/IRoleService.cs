using ControllerFirst.DTO.Requests;
using ControllerFirst.DTO.Responses;

namespace ControllerFirst.Services.Interfaces;

public interface IRoleService
{
    public Task<RoleResponse> CreateRoleAsync(CreateRoleRequest dto);
    public Task<RoleResponse> GetRoleAsync(string name);
    public Task<PaginatedResult<RoleResponse>> GetRolesAsync(int page = 1, int pageSize = 10);
    public Task MapRoleToUserAsync(string id, string roleName);
    public Task UnMapRoleToUserAsync(string id, string roleName);
    public Task<RoleResponse> UpdateRoleAsync(UpdateRoleRequest request);
    public Task<RoleResponse> DeleteRoleAsync(DeleteRoleRequest request);
    
} 