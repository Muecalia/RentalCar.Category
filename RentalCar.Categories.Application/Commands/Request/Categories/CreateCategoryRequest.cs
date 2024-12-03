using MediatR;
using RentalCar.Categories.Application.Commands.Response.Categories;
using RentalCar.Categories.Application.Wrappers;

namespace RentalCar.Categories.Application.Commands.Request.Categories
{
    public class CreateCategoryRequest : IRequest<ApiResponse<InputCategoryResponse>>
    {
        public string Name { get; set; } = string.Empty;
        public decimal DialyPrice { get; set; } = decimal.Zero;
    }
}
