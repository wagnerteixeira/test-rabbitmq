namespace test_rabbitmq.Configuration;

public record RabbitMqConfiguration
{
    public string Host { get; set; }
    public string Queue { get; set; }
    public int Port { get; set; }
}