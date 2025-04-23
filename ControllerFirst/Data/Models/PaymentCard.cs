namespace ControllerFirst.Data.Models;

public class PaymentCard
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string CardHolderName { get; set; }
    public string EncryptedCardNumber { get; set; }
    public string EncryptedExpiryDate { get; set; }
    public string EncryptedCVC { get; set; }

    public Guid UserInfoId { get; set; }
    public UserInfo UserInfo { get; set; }
}