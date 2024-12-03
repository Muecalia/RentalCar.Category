using MediatR;
using RentalCar.Categories.Application.Commands.Response.Categories;
using RentalCar.Categories.Application.Wrappers;

namespace RentalCar.Categories.Application.Commands.Request.Categories
{
    public class DeleteCategoryRequest(string id) : IRequest<ApiResponse<InputCategoryResponse>>
    {
        public string Id { get; set; } = id;
    }
}
