using RentalCar.Categories.Core.Services;
using Serilog;

namespace RentalCar.Categories.Infrastructure.Services;

public class LoggerService : ILoggerService
{
  public LoggerService() { }

    public void LogError(string message)
    {
        Log.Error(message);
    }

    public void LogError(string message, Exception exception)
    {
        Log.Error(exception, message);
    }

    public void LogInformation(string message)
    {
        Log.Information(message);
    }

    public void LogWarning(string message)
    {
        Log.Warning(message);
    } 
}