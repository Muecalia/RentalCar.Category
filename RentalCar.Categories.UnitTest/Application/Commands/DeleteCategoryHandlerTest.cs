using FluentAssertions;
using Moq;
using RentalCar.Categories.Application.Commands.Request;
using RentalCar.Categories.Application.Handlers;
using RentalCar.Categories.Core.Entities;
using RentalCar.Categories.Core.Repositories;
using RentalCar.Categories.Core.Services;

namespace RentalCar.Categories.UnitTest.Application.Commands
{
    public class DeleteCategoryHandlerTest
    {
        [Fact]
        public async Task DeleteCategory_Executed_Return_InputCategoryResponse()
        {
            // Arrange
            var category = new Category
            {
                Id = "Id",
                Name = "Category 1",
                DailyPrice = 50
            };

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var loggerServiceMock = new Mock<ILoggerService>();
            var prometheusServiceMock = new Mock<IPrometheusService>();

            categoryRepositoryMock.Setup(repo => repo.GetById(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(category);
            categoryRepositoryMock.Setup(repo => repo.Delete(It.IsAny<Category>(), It.IsAny<CancellationToken>()));
            prometheusServiceMock.Setup(service => service.AddDeleteCategoryCounter(It.IsAny<string>()));

            var deleteCategoryHandler = new DeleteCategoryHandler(categoryRepositoryMock.Object, loggerServiceMock.Object, prometheusServiceMock.Object);


            // Act
            var result = await deleteCategoryHandler.Handle(new DeleteCategoryRequest("Id"), It.IsAny<CancellationToken>());

            // Assert
            result.Data.Should().NotBeNull();
            result.Message.Should().NotBeNullOrEmpty();
            result.Succeeded.Should().BeTrue();

            categoryRepositoryMock.Verify(repo => repo.GetById(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            categoryRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
