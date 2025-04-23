namespace ControllerFirst.Data.Models;

public class UserInfo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; }

    public ICollection<DeliveryAddress> Addresses { get; set; } = new List<DeliveryAddress>();
    public ICollection<PaymentCard> Cards { get; set; } = new List<PaymentCard>();
}