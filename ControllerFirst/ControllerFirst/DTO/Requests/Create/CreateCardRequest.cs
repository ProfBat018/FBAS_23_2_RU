namespace ControllerFirst.DTO.Requests;

public record CreateCardRequest(string number, string expirationDate, string cvv);
