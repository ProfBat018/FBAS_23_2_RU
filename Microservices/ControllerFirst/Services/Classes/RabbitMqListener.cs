using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ControllerFirst.Services.Classes;

public class RabbitMqListener : BackgroundService
{
    private readonly RabbitMqSettings _settings;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IModel? _channel;
    private string _queue = "weather";

    public RabbitMqListener(IOptions<RabbitMqSettings> options)
    {
        _settings = options.Value;
    public RabbitMqListener(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {

            HostName = _settings.Host,
            UserName = _settings.Username,
            Password = _settings.Password
        };
        _queue = _settings.Queue;
            HostName = _configuration["RabbitMQ:Host"] ?? "rabbitmq",
            UserName = _configuration["RabbitMQ:Username"],
            Password = _configuration["RabbitMQ:Password"]
        };
        _queue = _configuration["RabbitMQ:Queue"] ?? "weather";
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: _queue,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (ch, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"RabbitMQ message: {message}");
            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(queue: _queue, autoAck: false, consumer: consumer);
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}
