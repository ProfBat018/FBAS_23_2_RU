using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace WeatherApp;

public class RabbitMqService : IDisposable
{
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly string _queue;
    private readonly RabbitMqSettings _settings;

    public RabbitMqService(IOptions<RabbitMqSettings> options)
    {
        _settings = options.Value;
        var factory = new ConnectionFactory
        {
            HostName = _settings.Host,
            UserName = _settings.Username,
            Password = _settings.Password
        };
        _queue = _settings.Queue;

    public RabbitMqService(IConfiguration configuration)
    {
        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:Host"] ?? "rabbitmq",
            UserName = configuration["RabbitMQ:Username"],
            Password = configuration["RabbitMQ:Password"]
        };
        _queue = configuration["RabbitMQ:Queue"] ?? "weather";

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: _queue,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    public void Publish(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: string.Empty,
                             routingKey: _queue,
                             basicProperties: null,
                             body: body);
    }

    public void Dispose()
    {
        if (_channel.IsOpen) _channel.Close();
        _connection.Close();
    }
}
