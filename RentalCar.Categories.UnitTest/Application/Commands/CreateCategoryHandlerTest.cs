﻿using FluentAssertions;
using Moq;
using RentalCar.Categories.Application.Commands.Request;
using RentalCar.Categories.Application.Handlers;
using RentalCar.Categories.Core.Entities;
using RentalCar.Categories.Core.Repositories;
using RentalCar.Categories.Core.Services;

namespace RentalCar.Categories.UnitTest.Application.Commands
{
    public class CreateCategoryHandlerTest
    {
        [Fact]
        public async Task CreateCategory_Executed_return_InputCategoryResponse()
        {
            // Arrange
            var category = new Category
            {
                Id = "Id",
                Name = "Category 1",
                DailyPrice = 50
            };

            var createCategoryRequest = new CreateCategoryRequest 
            {
                DialyPrice = 50,
                Name = "Categoria 1"
            };

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var loggerServiceMock = new Mock<ILoggerService>();
            var prometheusServiceMock = new Mock<IPrometheusService>();

            categoryRepositoryMock.Setup(repo => repo.IsCategoryExist(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            categoryRepositoryMock.Setup(repo => repo.Create(It.IsAny<Category>(), It.IsAny<CancellationToken>())).ReturnsAsync(category);
            prometheusServiceMock.Setup(service => service.AddNewCategoryCounter(It.IsAny<string>()));

            //_prometheusService.AddCategoryCounter(HttpStatusCode.Created.ToString());
            var createCategoryHandler = new CreateCategoryHandler(categoryRepositoryMock.Object, loggerServiceMock.Object, prometheusServiceMock.Object);
            
            // Act
            var result = await createCategoryHandler.Handle(createCategoryRequest, It.IsAny<CancellationToken>());

            // Assert
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();

            categoryRepositoryMock.Verify(repo => repo.IsCategoryExist(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            categoryRepositoryMock.Verify(repo => repo.Create(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
