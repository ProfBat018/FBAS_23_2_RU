namespace ControllerFirst.DTO.Requests;

public record CreateAddressRequest(string country, string city,
string street, string zipCode, bool isDefault);