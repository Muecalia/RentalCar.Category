using FluentAssertions;
using Moq;
using RentalCar.Categories.Application.Commands.Request;
using RentalCar.Categories.Application.Handlers;
using RentalCar.Categories.Core.Entities;
using RentalCar.Categories.Core.Repositories;
using RentalCar.Categories.Core.Services;

namespace RentalCar.Categories.UnitTest.Application.Commands
{
    public class UpdadeCategoryHandlerTest
    {
        [Fact]
        public async Task UpdadeCategory_Executed_Return_InputCategoryResponse()
        {
            // Arrange
            var category = new Category
            {
                Id = "Id",
                Name = "Category 1",
                DailyPrice = 50
            };

            var updateCategoryRequest = new UpdateCategoryRequest 
            {
                Id = "Id",
                Name = "Name",
                DialyPrice = 50,
            };

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var loggerServiceMock = new Mock<ILoggerService>();
            var prometheusServiceMock = new Mock<IPrometheusService>();

            categoryRepositoryMock.Setup(repo => repo.GetById(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(category);
            categoryRepositoryMock.Setup(repo => repo.Update(It.IsAny<Category>(), It.IsAny<CancellationToken>()));
            prometheusServiceMock.Setup(service => service.AddUpdateCategoryCounter(It.IsAny<string>()));

            var updadeCategoryHandler = new UpdadeCategoryHandler(categoryRepositoryMock.Object, loggerServiceMock.Object, prometheusServiceMock.Object);

            // Act
            var result = await updadeCategoryHandler.Handle(updateCategoryRequest, It.IsAny<CancellationToken>());

            // Assert
            result.Data.Should().NotBeNull();
            result.Message.Should().NotBeNullOrEmpty();
            result.Succeeded.Should().BeTrue();

            categoryRepositoryMock.Verify(repo => repo.GetById(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            categoryRepositoryMock.Verify(repo => repo.Update(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);

        }
    }
}
