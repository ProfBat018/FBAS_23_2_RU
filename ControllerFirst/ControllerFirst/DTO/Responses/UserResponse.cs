namespace ControllerFirst.DTO.Responses;

public record UserResponse(Guid id, string username, string email, bool emailConfirmed);