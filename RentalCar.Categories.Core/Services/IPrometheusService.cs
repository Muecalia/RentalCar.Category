namespace RentalCar.Categories.Core.Services;

public interface IPrometheusService
{
    void AddCategoryCounter(string statusCode);
}