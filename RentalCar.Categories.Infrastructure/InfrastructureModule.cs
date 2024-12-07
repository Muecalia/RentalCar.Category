using Microsoft.Extensions.DependencyInjection;
using RentalCar.Categories.Core.Repositories;
using RentalCar.Categories.Core.Services;
using RentalCar.Categories.Infrastructure.MessageBus;
using RentalCar.Categories.Infrastructure.Prometheus;
using RentalCar.Categories.Infrastructure.Repositories;
using RentalCar.Categories.Infrastructure.Services;

namespace RentalCar.Categories.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) 
    {
        services
            .AddServices();
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services) 
    {
        services.AddSingleton<ILoggerService, LoggerService>();
        services.AddSingleton<IRabbitMqService, RabbitMqService>();

        services.AddSingleton<IPrometheusService, PrometheusService>();

        services.AddScoped<ICategoryRepository, CategoryRepository>();

        return services;
    }
}