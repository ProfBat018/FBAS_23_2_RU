namespace ControllerFirst.DTO.Requests;

public record ChangePasswordRequest(string oldPassword, string newPassword);