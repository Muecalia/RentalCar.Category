namespace RentalCar.Categories.Application.Queries.Response;

public class GetService(string id, string name)
{
    public string Id { get; set; } = id;
    public string Name { get; set; } = name;
}