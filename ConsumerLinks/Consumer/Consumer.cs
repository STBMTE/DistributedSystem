using ConsumerLinks.Http;
using ConsumerLinks.Models;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ConsumerLinks.Consumer
{
    public class Consumer : BackgroundService
    {
        private IServiceProvider _sp;
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;
        public Consumer(IServiceProvider sp)
        {
            _sp = sp;

            _factory = new ConnectionFactory() { HostName = "rabbitmq" };

            _connection = _factory.CreateConnection();

            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: "links",
                durable: true,
                exclusive: false);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                _channel.Dispose();
                _connection.Dispose();
                return Task.CompletedTask;
            }

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();

                string str = Encoding.UTF8.GetString(body);

                var link = JsonSerializer.Deserialize<Link>(Encoding.UTF8.GetString(body));

                Console.WriteLine(link);

                Task.Run(async () =>
                {
                    using (var scope = _sp.CreateScope())
                    {
                        var httpService = scope.ServiceProvider.GetRequiredService<IHttpService>();

                        var code = await httpService.Send(link.URL);

                        await httpService.UpdateLink(new StatusUpdate(link.Id, true, code));

                        Console.WriteLine($"Status of link with {link.Id} has been changed on {link.statusCode}");
                    }
                });

            };

            _channel.BasicConsume(queue: "links", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
