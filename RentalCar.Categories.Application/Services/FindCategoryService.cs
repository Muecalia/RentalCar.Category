using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RentalCar.Categories.Application.Queries.Response;
using RentalCar.Categories.Core.Configs;
using RentalCar.Categories.Core.MessageBus;
using RentalCar.Categories.Core.Repositories;
using RentalCar.Categories.Core.Services;

namespace RentalCar.Categories.Application.Services;

public class FindCategoryService : BackgroundService
{
    private readonly IRabbitMqService _rabbitMqService;
    private readonly ILoggerService _loggerService;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FindCategoryService(IRabbitMqService rabbitMqService, ILoggerService loggerService, IServiceScopeFactory serviceScopeFactory)
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

            await GetCategory(scopeService, cancellationToken);

            Console.WriteLine("Category Service is running");
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        }
    }
    
    private async Task GetCategory(ICategoryRepository repository, CancellationToken cancellationToken)
    {
        const string title = "Pesquisar categoria";
        
        using var connection = await _rabbitMqService.CreateConnection(cancellationToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        try
        {
            await channel.QueueDeclareAsync(RabbitQueue.CATEGORY_MODEL_FIND_REQUEST_QUEUE, true, false, false, null, cancellationToken: cancellationToken);
            await channel.BasicQosAsync(0, 1, false, cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var idCategory= JsonSerializer.Deserialize<string>(message);

                if (idCategory is null)
                {
                    _loggerService.LogWarning(MessageError.NotFound(title));
                    return;
                }
                
                Console.WriteLine($"request -> Id Category: {idCategory}");
                var manufacturer = await repository.GetById(idCategory, cancellationToken);
                if (manufacturer is not null)
                {
                    var response = new GetService(manufacturer.Id, manufacturer.Name);
                    Console.WriteLine($"response -> Id Category: {response.Id} - Id Model: {response.Name}");
                    await _rabbitMqService.PublishMessage(response, RabbitQueue.CATEGORY_MODEL_FIND_RESPONSE_QUEUE, cancellationToken);
                }
            };

            await channel.BasicConsumeAsync(RabbitQueue.CATEGORY_MODEL_FIND_REQUEST_QUEUE, autoAck: true, consumer: consumer, cancellationToken: cancellationToken);
            await Task.Delay(TimeSpan.FromMicroseconds(30), cancellationToken);
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