namespace ControllerFirst.DTO.Requests;

public record UpdateAddressRequest(string addressId, string country, string city,
    string street, string zipCode, bool isDefault);