using FluentAssertions;
using Moq;
using RentalCar.Categories.Application.Commands.Request.Categories;
using RentalCar.Categories.Application.Handlers.Categories;
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

            categoryRepositoryMock.Setup(repo => repo.GetById(It.IsAny<string>(), new CancellationToken())).ReturnsAsync(category);
            categoryRepositoryMock.Setup(repo => repo.Delete(It.IsAny<Category>(), new CancellationToken()));

            var deleteCategoryHandler = new DeleteCategoryHandler(categoryRepositoryMock.Object, loggerServiceMock.Object);

            // Act
            var result = await deleteCategoryHandler.Handle(new DeleteCategoryRequest("Id"), new CancellationToken());

            // Assert
            result.Data.Should().NotBeNull();
            result.Message.Should().NotBeNullOrEmpty();
            result.Succeeded.Should().BeTrue();

            categoryRepositoryMock.Verify(repo => repo.GetById(It.IsAny<string>(), new CancellationToken()), Times.Once);
            categoryRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Category>(), new CancellationToken()), Times.Once);
        }

    }
}
