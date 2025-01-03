using System.Diagnostics.Metrics;
using RentalCar.Categories.Core.Services;

namespace RentalCar.Categories.Infrastructure.Prometheus;

public class PrometheusService : IPrometheusService
{
    private readonly Meter _meter;
    private readonly Counter<int> _counter;
    private readonly ObservableGauge<int> _totalOrderGauge;
    
    public PrometheusService()
    {
        _meter = new Meter("RentalCar.Category");
        _counter = _meter.CreateCounter<int>("category_total_request", "status_code");
        //_totalOrderGauge = _meter.CreateObservableGauge("total_orders", () => new Measurement<int>() )
    }
    
    public void AddCategoryCounter(string statusCode)
    {
        _counter.Add(1, KeyValuePair.Create<string, object?>("category_error_code",statusCode));
    }

    public void AddNewCategoryCounter(string statusCodes)
    {
        System.Diagnostics.Debug.Print(statusCodes);
    }

    public void AddDeleteCategoryCounter(string statusCodes)
    {
        System.Diagnostics.Debug.Print(statusCodes);
    }

    public void AddUpdateCategoryCounter(string statusCodes)
    {
        System.Diagnostics.Debug.Print(statusCodes);
    }

    public void AddUpdateStatusCategoryCounter(string statusCodes)
    {
        System.Diagnostics.Debug.Print(statusCodes);
    }

    public void AddFindByIdCategoryCounter(string statusCodes)
    {
        System.Diagnostics.Debug.Print(statusCodes);
    }

    public void AddFindAllCategorysCounter(string statusCodes)
    {
        System.Diagnostics.Debug.Print(statusCodes);
    }
    
}