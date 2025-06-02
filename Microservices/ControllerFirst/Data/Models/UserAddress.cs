namespace ControllerFirst.Data.Models;

public class UserAddress
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string PostalCode { get; set; }
    
    public bool IsPrimary { get; set; }

    public User User { get; set; }
}