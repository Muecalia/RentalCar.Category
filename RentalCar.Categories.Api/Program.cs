using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RentalCar.Categories.Api.Endpoints;
using RentalCar.Categories.Application;
using RentalCar.Categories.Core.Configs;
using RentalCar.Categories.Infrastructure;
using RentalCar.Categories.Infrastructure.Persistence;
using Serilog;

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
    
    //Opentelemry
    const string serviceName = "category";
    const string serviceVersion = "v1";
    //const string endPoint = "http://localhost:5299";
    const string endPoint = "https://localhost:7163";

    builder.Services.AddOpenTelemetry()
        .ConfigureResource(resource => resource.AddService(serviceName))
        .WithTracing(tracing => tracing
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService(serviceName: serviceName, serviceVersion:serviceVersion))
           .AddAspNetCoreInstrumentation()
           .AddOtlpExporter()
           .AddConsoleExporter())
        .WithMetrics(metrics => metrics
            .AddConsoleExporter()
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddPrometheusExporter()
        );

    
    /*builder.Logging.AddOpenTelemetry(options =>
    {
        options
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(serviceName))
            .AddConsoleExporter();
    });*/
    
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
    
    app.UseOpenTelemetryPrometheusScrapingEndpoint();
    
    app.UseCors(MyAllowSpecificOrigins);
    
    app.UseHttpsRedirection();
    
    app.UseAuthorization();
    
    app.Run();
