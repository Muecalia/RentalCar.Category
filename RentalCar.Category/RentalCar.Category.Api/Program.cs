using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RentalCar.Categories.Api.Endpoints;
using RentalCar.Categories.Application;
using RentalCar.Categories.Core.Configs;
using RentalCar.Categories.Infrastructure;
using RentalCar.Categories.Infrastructure.Persistence;
using Serilog;
using System.Text;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

//LOG
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("C:/RentalCar/Logs/RentalCarLocation.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Connection String
builder.Services.AddDbContextPool<RentalCarCategoryContext>(opt =>
    opt.UseMySql(builder.Configuration.GetConnectionString("RentalCarCategoryConnetion"), new MySqlServerVersion(new Version(8, 0, 40)))
);

//RabbitMQ
builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection("RabbitMqConfig"));

//InfrastructureModule
builder.Services.AddInfrastructure();

//ApplicationModule
builder.Services.AddApplication();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RentalCar.Category.API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header usando o esquema Bearer."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
         {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
         }
    });
});

builder.Services
  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,

          ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
          ValidAudience = builder.Configuration["JwtConfig:Audience"],
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:SecretKey"]))
      };
  });

// Exception
//builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddProblemDetails();

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
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RentalCar.Category.API v1"));
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

//app.MapControllers();

app.Run();
