namespace ControllerFirst.Data.Models;

public class UserCard
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string CardHolder { get; set; }
    public string CardNumberEncrypted { get; set; }
    public int ExpirationMonth { get; set; }
    public int ExpirationYear { get; set; }
    public string CVVEncrypted { get; set; }
    
    public bool IsDefault { get; set; }

    public CardType CardType { get; set; }

    public User User { get; set; }
}