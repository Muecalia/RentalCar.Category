using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using RentalCar.Categories.Application.Handlers;
using RentalCar.Categories.Application.Services;
using RentalCar.Categories.Application.Validators;

namespace RentalCar.Categories.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services
                .AddFluentValidation()
                .AddHandlers()
                .AddBackgroundServices();
            return services;
        }


        private static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining<CreateCategoryValidator>();

            return services;
        }

        private static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<CreateCategoryHandler>());

            return services;
        }
        
        private static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            services.AddHostedService<CategoryService>();
            services.AddHostedService<FindCategoryService>();
            return services;
        }

    }
}
