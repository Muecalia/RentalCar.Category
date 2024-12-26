using MediatR;
using RentalCar.Categories.Application.Commands.Response;
using RentalCar.Categories.Core.Wrappers;

namespace RentalCar.Categories.Application.Commands.Request
{
    public class UpdateCategoryRequest : IRequest<ApiResponse<InputCategoryResponse>>
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal DialyPrice { get; set; } = decimal.Zero;
    }
}
