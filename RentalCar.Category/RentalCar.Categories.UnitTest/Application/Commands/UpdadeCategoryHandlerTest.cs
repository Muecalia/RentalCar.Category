using FluentAssertions;
using Moq;
using RentalCar.Categories.Application.Commands.Request.Categories;
using RentalCar.Categories.Application.Handlers.Categories;
using RentalCar.Categories.Core.Entities;
using RentalCar.Categories.Core.Repositories;
using RentalCar.Categories.Core.Services;

namespace RentalCar.Categories.UnitTest.Application.Commands
{
    public class UpdadeCategoryHandlerTest
    {
        [Fact]
        public async void UpdadeCategory_Executed_Return_InputCategoryResponse()
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

            var _categoryRepositoryMock = new Mock<ICategoryRepository>();
            var _loggerServiceMock = new Mock<ILoggerService>();

            _categoryRepositoryMock.Setup(repo => repo.GetById(It.IsAny<string>(), new CancellationToken())).ReturnsAsync(category);
            _categoryRepositoryMock.Setup(repo => repo.Update(It.IsAny<Category>(), new CancellationToken()));

            var updadeCategoryHandler = new UpdadeCategoryHandler(_categoryRepositoryMock.Object, _loggerServiceMock.Object);

            // Act
            var result = await updadeCategoryHandler.Handle(updateCategoryRequest, new CancellationToken());

            // Assert
            result.Data.Should().NotBeNull();
            result.Message.Should().NotBeNullOrEmpty();
            result.Succeeded.Should().BeTrue();

            _categoryRepositoryMock.Verify(repo => repo.GetById(It.IsAny<string>(), new CancellationToken()), Times.Once);
            _categoryRepositoryMock.Verify(repo => repo.Update(It.IsAny<Category>(), new CancellationToken()), Times.Once);

        }
    }
}
