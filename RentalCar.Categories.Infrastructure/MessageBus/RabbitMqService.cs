using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RentalCar.Categories.Core.Configs;
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
            //RequestedConnectionTimeout = TimeSpan.FromSeconds(60)
        };

        var endpoints = new List<AmqpTcpEndpoint> {
            new AmqpTcpEndpoint("host.docker.internal")
        };

        return await factory.CreateConnectionAsync(endpoints, cancellationToken);
    }

    public async Task FecharConexao(IConnection connection, IChannel channel, CancellationToken cancellationToken)
    {
        try
        {
            await channel.CloseAsync(cancellationToken);
            await connection.CloseAsync(cancellationToken);
            _loggerService.LogInformation("RabbitMqService -> Conexão fechada com sucesso");
        }
        catch (Exception ex)
        {
            _loggerService.LogError($"RabbitMqService -> Erro ao fechar a conexão. Mensagem: {ex.Message}");
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

            _loggerService.LogInformation("RabbitMqService -> Sucesso ao publicar a mensagem.");
        }
        catch (Exception ex)
        {
            _loggerService.LogError("RabbitMqService -> Erro ao publicar a mensagem.", ex);
            throw;
        }
        finally
        {
            await FecharConexao(connection, channel, cancellationToken);
        }
    }
}