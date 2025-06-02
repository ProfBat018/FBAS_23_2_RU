namespace ControllerFirst.DTO.Responses;
public class CardResponse
{
    public string Id { get; set; }
    public string CardHolder { get; set; }
    public bool IsDefault { get; set; }
    public string Last4 { get; set; }
    public string CardType { get; set; }
}