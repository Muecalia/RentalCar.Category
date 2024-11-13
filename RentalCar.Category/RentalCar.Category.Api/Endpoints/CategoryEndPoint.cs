using MediatR;
using Microsoft.AspNetCore.Authorization;
using RentalCar.Categories.Application.Commands.Request.Categories;
using RentalCar.Categories.Application.Queries.Request.Categories;

namespace RentalCar.Categories.Api.Endpoints
{
    public static class CategoryEndPoint
    {
        public static void MapCategoryEndpoints(this IEndpointRouteBuilder route)
        {
            // Get All Categories
            route.MapGet("/GetAll", [Authorize(Roles = "Admin")] async (IMediator mediator, CancellationToken cancellationToken) => 
            {
                var result = await mediator.Send(new FindAllCategoriesRequest(), cancellationToken);
                return Results.Ok(result);
            });

            //Get category by Id
            route.MapGet("/GetById/{id}", [Authorize(Roles = "Admin")] async (string id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new FindCategoryByIdRequest(id), cancellationToken);
                return result.Succeeded ? Results.Ok(result) : Results.NotFound(result.Message);
            });

            // Create new category
            route.MapPost("/Create", [Authorize(Roles = "Admin")] async (CreateCategoryRequest request, IMediator mediator, CancellationToken cancellationToken) => 
            {
                var result = await mediator.Send(request, cancellationToken);
                return result.Succeeded ? Results.Created("", result) : Results.BadRequest(result.Message);
            });

            // Delete category
            route.MapDelete("/Delete/{id}", [Authorize(Roles = "Admin")] async (string id, IMediator mediator, CancellationToken cancellationToken) => 
            {
                var result = await mediator.Send(new DeleteCategoryRequest(id), cancellationToken);
                return result.Succeeded ? Results.Ok(result) : Results.NotFound(result.Message);
            });

            // Update category
            route.MapPut("/Update/{id}", [Authorize(Roles = "Admin")] async (string id, IMediator mediator, UpdateCategoryRequest request, CancellationToken cancellationToken) =>
            {
                request.Id = id;
                var result = await mediator.Send(request, cancellationToken);
                return result.Succeeded ? Results.Created("", result) : Results.BadRequest(result.Message);
            });
        }
    }
}
