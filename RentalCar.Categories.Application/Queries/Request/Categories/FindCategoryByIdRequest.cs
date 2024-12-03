using MediatR;
using RentalCar.Categories.Application.Queries.Response.Categories;
using RentalCar.Categories.Application.Wrappers;

namespace RentalCar.Categories.Application.Queries.Request.Categories
{
    public class FindCategoryByIdRequest(string id) : IRequest<ApiResponse<FindCategoryResponse>>
    {
        public string Id { get; set; } = id;
    }
}
