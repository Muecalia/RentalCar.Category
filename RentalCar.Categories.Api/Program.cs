using Microsoft.EntityFrameworkCore;
using RentalCar.Categories.Api;
using RentalCar.Categories.Api.Endpoints;
using RentalCar.Categories.Application;
using RentalCar.Categories.Core.Configs;
using RentalCar.Categories.Infrastructure;
using RentalCar.Categories.Infrastructure.Persistence;
using Serilog;

//namespace RentalCar.Categories.Api;

//public class Program
//{
    //public static void Main(string[] args)
    //{
    var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    //LOG
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .WriteTo.File("/var/log/rental_car_category.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();
    
    var builder = WebApplication.CreateBuilder(args);

    //RabbitMQ
    builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection("RabbitMqConfig"));

    // Add services to the container.
    builder.Services.AddAuthorization();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    //Connection String
    builder.Services.AddDbContextPool<RentalCarCategoryContext>(opt => 
        opt.UseMySql(builder.Configuration.GetConnectionString("CategoryConnection"), new MySqlServerVersion(new Version(8, 0, 40))));
    
    //InfrastructureModule
    builder.Services.AddInfrastructure();
    
    //ApplicationModule
    builder.Services.AddApplication();
    
    //Authorization
    builder.Services.AddAuthorization();

    //CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(MyAllowSpecificOrigins,
            policy =>
            {
                policy.WithOrigins("http://example.com", "http://www.contoso.com")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });

    var app = builder.Build();

    // Mapear endpoints
    app.MapCategoryEndpoints();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RentalCar Categoria v1"));
    }

    app.UseCors(MyAllowSpecificOrigins);

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.Run();
    //}
//}