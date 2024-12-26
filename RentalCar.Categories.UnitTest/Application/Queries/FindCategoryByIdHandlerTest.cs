using FluentAssertions;
using Moq;
using RentalCar.Categories.Application.Handlers;
using RentalCar.Categories.Application.Queries.Request;
using RentalCar.Categories.Core.Entities;
using RentalCar.Categories.Core.Repositories;
using RentalCar.Categories.Core.Services;

namespace RentalCar.Categories.UnitTest.Application.Queries
{
    public class FindCategoryByIdHandlerTest
    {
        [Fact]
        public async Task FindCategoryById_Executed_Return_FindCategoryResponse()
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
            prometheusServiceMock.Setup(service => service.AddFindByIdCategoryCounter(It.IsAny<string>()));

            var findCategoryByIdHandler = new FindCategoryByIdHandler(categoryRepositoryMock.Object, loggerServiceMock.Object, prometheusServiceMock.Object);

            // Act
            var result = await findCategoryByIdHandler.Handle(new FindCategoryByIdRequest("Id"), It.IsAny<CancellationToken>());

            // Assert
            result.Data.Should().NotBeNull();
            result.Message.Should().NotBeNullOrEmpty();
            result.Succeeded.Should().BeTrue();

            categoryRepositoryMock.Verify(repo => repo.GetById(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
          
        }
    }
}
