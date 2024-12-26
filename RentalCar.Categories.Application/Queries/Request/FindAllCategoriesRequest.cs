using MediatR;
using RentalCar.Categories.Application.Queries.Response;
using RentalCar.Categories.Core.Wrappers;

namespace RentalCar.Categories.Application.Queries.Request
{
    public class FindAllCategoriesRequest(int pageNumber, int pageSize) : IRequest<PagedResponse<FindCategoryResponse>>
    {
        public int PageNumber { get; set; } = pageNumber;
        public int PageSize { get; set; } = pageSize;
    }
}
