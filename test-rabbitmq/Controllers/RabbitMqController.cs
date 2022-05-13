using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using test_rabbitmq.Configuration;
using test_rabbitmq.Models;

namespace test_rabbitmq.Controllers;

[ApiController]
[Route("[controller]")]
public class RabbitMqController : ControllerBase
{
    private readonly ConnectionFactory _connectionFactory;
    private readonly RabbitMqConfiguration _rabbitMqConfiguration;

    public RabbitMqController(IOptions<RabbitMqConfiguration> options)
    {
        _rabbitMqConfiguration = options.Value;
        _connectionFactory = new()
        {
            HostName = _rabbitMqConfiguration.Host,
            UserName = "wagner-mq",
            Password = "6H96dBOXIDqD",
            Port = _rabbitMqConfiguration.Port
        };
    }
    [HttpPost(Name = "producer")]
    public IActionResult Producer(Message message)
    {
        using (var connection = _connectionFactory.CreateConnection())
        {
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: _rabbitMqConfiguration.Queue,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var jsonMessage = JsonSerializer.Serialize(message);
                var bytesMessage = Encoding.UTF8.GetBytes(jsonMessage);

                channel.BasicPublish(
                    exchange: "",
                    routingKey: _rabbitMqConfiguration.Queue,
                    basicProperties: null,
                    body: bytesMessage);
            }
        }
        
        return Accepted();
    }
}