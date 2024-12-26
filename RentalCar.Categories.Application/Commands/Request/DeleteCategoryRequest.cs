using MediatR;
using RentalCar.Categories.Application.Commands.Response;
using RentalCar.Categories.Core.Wrappers;

namespace RentalCar.Categories.Application.Commands.Request
{
    public class DeleteCategoryRequest(string id) : IRequest<ApiResponse<InputCategoryResponse>>
    {
        public string Id { get; set; } = id;
    }
}
