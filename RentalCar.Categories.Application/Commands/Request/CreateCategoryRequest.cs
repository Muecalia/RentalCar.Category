using MediatR;
using RentalCar.Categories.Application.Commands.Response;
using RentalCar.Categories.Core.Wrappers;

namespace RentalCar.Categories.Application.Commands.Request
{
    public class CreateCategoryRequest : IRequest<ApiResponse<InputCategoryResponse>>
    {
        public string Name { get; set; } = string.Empty;
        public decimal DialyPrice { get; set; } = decimal.Zero;
    }
}
