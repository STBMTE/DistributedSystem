namespace DistributedSystem.RabbitMQ
{
    public interface IRabbitMqService
    {
        void SendMessage<T>(T message);
    }
}
