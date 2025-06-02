using ControllerFirst.DTO.Requests;
using ControllerFirst.DTO.Responses;

namespace ControllerFirst.Services.Interfaces;

public interface IAccountService
{
    public Task RegisterAsync(RegisterRequest request);
    public Task ConfirmEmailAsync(string username, HttpContext context);
    public Task VerifyEmailAsync(string token);
    
    public Task ChangeUserNameAsync(string oldUsername, string newUsername);

    public Task ResetPasswordAsync(ResetPasswordRequest request);
    
    public Task ChangeEmailAsync(ChangeEmailRequest request);

    public Task DeleteAccountAsync(string id);
    
    public Task ChangePasswordAsync(string username, ChangePasswordRequest request);

    public Task<UserInfoResponse> GetProfileAsync(string username);
}