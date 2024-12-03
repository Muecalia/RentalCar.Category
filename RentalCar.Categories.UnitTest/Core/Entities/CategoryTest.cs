using FluentAssertions;
using RentalCar.Categories.Core.Entities;

namespace RentalCar.Categories.UnitTest.Core.Entities
{
    public class CategoryTest
    {
        [Fact]
        public void Category_Success()
        {
            // Arrange
            var category = new Category 
            {
                Id = "Id",
                Name = "Category 1",
                DailyPrice = 50                
            };

            // Act


            //Assert
            category.Should().NotBeNull();
            category.Name.Should().NotBeNullOrEmpty();
            category.CreatedAt.Date.Should().Be(DateTime.Now.Date);
        }
    }
}
