namespace ControllerFirst.DTO.Responses;

public class NotificationMessage
{
    public string Type { get; set; }
    public object Payload { get; set; }

    public NotificationMessage(string type, object payload)
    {
        Type = type;
        Payload = payload;
    }
}