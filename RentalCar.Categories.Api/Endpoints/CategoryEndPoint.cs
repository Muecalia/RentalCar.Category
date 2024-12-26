using MediatR;
using Microsoft.AspNetCore.Authorization;
using RentalCar.Categories.Application.Commands.Request;
using RentalCar.Categories.Application.Queries.Request;

namespace RentalCar.Categories.Api.Endpoints
{
    public static class CategoryEndPoint
    {
        public static void MapCategoryEndpoints(this IEndpointRouteBuilder route)
        {
            // Get All Categories
            route.MapGet("/category", [Authorize(Roles = "Admin")] async (IMediator mediator, CancellationToken cancellationToken, int pageNumber = 1, int pageSize = 10) => 
            {
                var result = await mediator.Send(new FindAllCategoriesRequest(pageNumber, pageSize), cancellationToken);
                return Results.Ok(result);
            });

            //Get category by id
            route.MapGet("/category/{id}", [Authorize(Roles = "Admin")] async (string id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new FindCategoryByIdRequest(id), cancellationToken);
                return result.Succeeded ? Results.Ok(result) : Results.NotFound(result.Message);
            });

            // Create new category
            route.MapPost("/category", [Authorize(Roles = "Admin")] async (CreateCategoryRequest request, IMediator mediator, CancellationToken cancellationToken) => 
            {
                var result = await mediator.Send(request, cancellationToken);
                return result.Succeeded ? Results.Created("", result) : Results.BadRequest(result.Message);
            });

            // Delete category
            route.MapDelete("/category/{id}", [Authorize(Roles = "Admin")] async (string id, IMediator mediator, CancellationToken cancellationToken) => 
            {
                var result = await mediator.Send(new DeleteCategoryRequest(id), cancellationToken);
                return result.Succeeded ? Results.Ok(result) : Results.NotFound(result.Message);
            });

            // Update category
            route.MapPut("/category/{id}", [Authorize(Roles = "Admin")] async (string id, IMediator mediator, UpdateCategoryRequest request, CancellationToken cancellationToken) =>
            {
                request.Id = id;
                var result = await mediator.Send(request, cancellationToken);
                return result.Succeeded ? Results.Ok(result) : Results.BadRequest(result.Message);
            });
        }
    }
}
