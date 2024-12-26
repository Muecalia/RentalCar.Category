using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RentalCar.Categories.Core.Configs;
using RentalCar.Categories.Core.MessageBus;
using RentalCar.Categories.Core.Services;

namespace RentalCar.Categories.Infrastructure.MessageBus;

public class RabbitMqService : IRabbitMqService
{
    private RabbitMqConfig _rabbitMqConfig { get; }
    private readonly ILoggerService _loggerService;

    public RabbitMqService(IOptions<RabbitMqConfig> config, ILoggerService loggerService)
    {
        _rabbitMqConfig = config.Value;
        _loggerService = loggerService;
    }

    public async Task<IConnection> CreateConnection(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMqConfig.HostName,
            UserName = _rabbitMqConfig.UserName,
            Password = _rabbitMqConfig.Password
        };

        return await factory.CreateConnectionAsync(cancellationToken);
    }

    public async Task CloseConnection(IConnection connection, IChannel channel, CancellationToken cancellationToken)
    {
        try
        {
            await channel.CloseAsync(cancellationToken);
            await channel.DisposeAsync();
            await connection.CloseAsync(cancellationToken);
            await connection.DisposeAsync();
        }
        catch (Exception ex)
        {
            _loggerService.LogInformation(MessageError.FecharConexão(ex.Message));
            throw;
        }
    }

    public async Task PublishMessage<T>(T message, string queue, CancellationToken cancellationToken)
    {
        using var connection = await CreateConnection(cancellationToken);

        using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        try
        {
            await channel.QueueDeclareAsync(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);

            var json = JsonSerializer.Serialize(message);

            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(exchange: "", routingKey: queue, body: body, cancellationToken);

            _loggerService.LogInformation($"Sucesso ao publicar a mensagem na fila {queue}.");
        }
        catch (Exception ex)
        {
            _loggerService.LogError($"Error ao publicar a mensagem. Mensagem: {ex.Message}");
            throw;
        }
        finally
        {
            await CloseConnection(connection, channel, cancellationToken);
        }
    }
}