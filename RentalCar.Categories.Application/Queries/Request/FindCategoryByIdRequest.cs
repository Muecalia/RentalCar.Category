using MediatR;
using RentalCar.Categories.Application.Queries.Response;
using RentalCar.Categories.Core.Wrappers;

namespace RentalCar.Categories.Application.Queries.Request
{
    public class FindCategoryByIdRequest(string id) : IRequest<ApiResponse<FindCategoryResponse>>
    {
        public string Id { get; set; } = id;
    }
}
