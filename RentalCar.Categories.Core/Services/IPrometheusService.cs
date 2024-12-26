namespace RentalCar.Categories.Core.Services;

public interface IPrometheusService
{
    void AddNewCategoryCounter(string statusCodes);
    void AddDeleteCategoryCounter(string statusCodes);
    void AddUpdateCategoryCounter(string statusCodes);
    void AddUpdateStatusCategoryCounter(string statusCodes);
    void AddFindByIdCategoryCounter(string statusCodes);
    void AddFindAllCategorysCounter(string statusCodes);
}