using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RentalCar.Categories.Application.Commands.Request;
using RentalCar.Categories.Core.Configs;
using RentalCar.Categories.Core.MessageBus;
using RentalCar.Categories.Core.Repositories;
using RentalCar.Categories.Core.Services;

namespace RentalCar.Categories.Application.Services;

public class CategoryService : BackgroundService
{
    private readonly IRabbitMqService _rabbitMqService;
    private readonly ILoggerService _loggerService;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CategoryService(IRabbitMqService rabbitMqService, ILoggerService loggerService, IServiceScopeFactory serviceScopeFactory)
    {
        _rabbitMqService = rabbitMqService;
        _loggerService = loggerService;
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    protected async override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var scopedFactory = _serviceScopeFactory.CreateScope();
            var scopeService = scopedFactory.ServiceProvider.GetService<ICategoryRepository>();

            await ValidateIdCategory(scopeService, cancellationToken);

            Console.WriteLine("Category Service is running");
            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }
    
    private async Task ValidateIdCategory(ICategoryRepository repository, CancellationToken cancellationToken)
    {
        const string title = "Validar Id Category";
        
        using var connection = await _rabbitMqService.CreateConnection(cancellationToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        try
        {
            await channel.QueueDeclareAsync(RabbitQueue.CATEGORY_MODEL_REQUEST_QUEUE, true, false, false, null, cancellationToken: cancellationToken);
            await channel.BasicQosAsync(0, 1, false, cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var request = JsonSerializer.Deserialize<RequestValidCategory>(message);

                if (request == null)
                {
                    _loggerService.LogWarning(MessageError.NotFound(title));
                    return;
                }
                
                Console.WriteLine($"request -> Id Category: {request.IdService} - Id Model: {request.IdModel}");
                var manufacturer = await repository.GetById(request.IdService, cancellationToken);
                if (manufacturer != null)
                {
                    var response = new RequestValidCategory(request.IdModel, manufacturer.Id);
                    Console.WriteLine($"response -> Id Category: {response.IdService} - Id Model: {response.IdModel}");
                    await _rabbitMqService.PublishMessage(response, RabbitQueue.CATEGORY_MODEL_RESPONSE_QUEUE, cancellationToken);
                }
            };

            await channel.BasicConsumeAsync(RabbitQueue.CATEGORY_MODEL_REQUEST_QUEUE, autoAck: true, consumer: consumer, cancellationToken: cancellationToken);
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        }
        catch (Exception ex)
        {
            _loggerService.LogError(title, ex);
            throw;
        }
        finally
        {
            await _rabbitMqService.CloseConnection(connection, channel, cancellationToken);
        }
    }
}