using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using System.Threading.Channels;

namespace DistributedSystem.RabbitMQ
{
    public class RabbitMqService : IRabbitMqService
    {
        public static IModel Channel { get => Channel; set => Channel = value; }
        public RabbitMqService() { }

        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/"
            };
            Console.WriteLine("SEND RABBIT DATA");
            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare("links", durable: true, exclusive: false);

            var jsonString = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonString);

            channel.BasicPublish("", "links", body: body);
        }
    }
}
