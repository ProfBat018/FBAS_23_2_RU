namespace ControllerFirst.DTO.Responses;

public record UserInfoResponse(string username, string email, bool isEmailConfirmed ,List<string> roles);   