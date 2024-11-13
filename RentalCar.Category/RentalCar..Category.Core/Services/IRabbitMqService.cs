using RabbitMQ.Client;

namespace RentalCar.Categories.Core.Services
{
    public interface IRabbitMqService
    {
        Task<IConnection> CreateConnection(CancellationToken cancellationToken);
        void PublishMessage<T>(T message, string queue, CancellationToken cancellationToken);
    }
}
