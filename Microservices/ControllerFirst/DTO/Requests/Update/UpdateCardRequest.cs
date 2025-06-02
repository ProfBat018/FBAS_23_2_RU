namespace ControllerFirst.DTO.Requests;

public record UpdateCardRequest(string cardId, string number, string holder, string expirationDate, string cvv, bool isDefault);

