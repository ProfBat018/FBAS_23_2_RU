namespace ControllerFirst.Services.Classes;

public class RabbitMqSettings
{
    public string Host { get; set; } = "rabbitmq";
    public string Username { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string Queue { get; set; } = "weather";
}
