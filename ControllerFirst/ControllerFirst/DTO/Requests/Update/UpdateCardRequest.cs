namespace ControllerFirst.DTO.Requests;

public record UpdateCardRequest(string cardId, string number, string expirationDate, string cvv);