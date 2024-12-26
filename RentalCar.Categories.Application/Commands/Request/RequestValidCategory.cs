namespace RentalCar.Categories.Application.Commands.Request;

public class RequestValidCategory(string idModel, string idService)
{
    public string IdModel { get; set; } = idModel;
    public string IdService { get; set; } = idService;
}