﻿using FluentAssertions;
using Moq;
using RentalCar.Categories.Application.Handlers.Categories;
using RentalCar.Categories.Application.Queries.Request.Categories;
using RentalCar.Categories.Core.Entities;
using RentalCar.Categories.Core.Repositories;
using RentalCar.Categories.Core.Services;

namespace RentalCar.Categories.UnitTest.Application.Queries
{
    public class FindAllCategoryHandlerTest
    {
        [Fact]
        public async Task FindAllCategories_Executed_Return_List_FindCategoryResponse()
        {
            // Arrange
            var categories = new List<Category> 
            {
                new() { Id = "Id 1", Name = "Name 1", DailyPrice = 50, CreatedAt = DateTime.Now },
                new() { Id = "Id 2", Name = "Name 2", DailyPrice = 75, CreatedAt = DateTime.Now },
                new() { Id = "Id 3", Name = "Name 3", DailyPrice = 100, CreatedAt = DateTime.Now },
                new() { Id = "Id 4", Name = "Name 4", DailyPrice = 150, CreatedAt = DateTime.Now },
            };

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var loggerServiceMock = new Mock<ILoggerService>();

            categoryRepositoryMock.Setup(repo => repo.GetAll(new CancellationToken())).ReturnsAsync(categories);

            var findAllCategoriesHandler = new FindAllCategoriesHandler(categoryRepositoryMock.Object, loggerServiceMock.Object);

            // Act
            var result = await findAllCategoriesHandler.Handle(new FindAllCategoriesRequest(), new CancellationToken());

            // Assert
            result.Datas.Should().NotBeNull();
            result.Message.Should().NotBeNullOrEmpty();
            result.Succeeded.Should().BeTrue();
            result.Datas.Count.Should().Be(categories.Count);

            categoryRepositoryMock.Verify(repo => repo.GetAll(new CancellationToken()), Times.Once);
        }
    }
}