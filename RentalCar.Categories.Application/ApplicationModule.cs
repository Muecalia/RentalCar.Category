using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using RentalCar.Categories.Application.Handlers.Categories;
using RentalCar.Categories.Application.Validators.Categories;

namespace RentalCar.Categories.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services
                .AddFluentValidation()
                .AddHandlers();
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

    }
}
