using ControllerFirst.DTO.Requests;
using ControllerFirst.DTO.Responses;

namespace ControllerFirst.Services.Interfaces;

public interface IUserService 
{
    public Task<PaginatedResult<UserResponse>> GetUsersAsync(int page=1, int pageSize=5);
    public Task<UserInfoResponse> GetUserInfoAsync(Guid Id);
    
    public Task<RolesInfoResponse> GetUserRolesAsync(Guid Id);
    
    public Task ConfirmEmailAsync(ConfirmFromAdminRequest request);
    
    public Task DeleteUserAsync(Guid id);
    
}