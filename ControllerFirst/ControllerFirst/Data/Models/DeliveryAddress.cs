namespace ControllerFirst.Data.Models;

public class DeliveryAddress
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string FullName { get; set; }
    public string PhoneNumber { get; set; }

    public string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }

    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; }

    public Guid UserInfoId { get; set; }
    public UserInfo UserInfo { get; set; }
}