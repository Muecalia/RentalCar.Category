using FluentAssertions;
using RentalCar.Employees.Core.Entities;

namespace RentalCar.Employees.UnitTests.Core.Entities
{
    public class EmployeeTest
    {
        [Fact]
        public void Employee_Correct()
        {
            // Arrange
            var employee = new Employee
            {
                Id = "Id",
                Name = "Name",
                Email = "email",
                Gender = 'F',
                Phone = "123456789",
                CreatedAt = DateTime.Now
            };

            // Act

            // Assert
            employee.Should().NotBeNull();
            employee.Name.Should().NotBeNullOrEmpty();
            employee.Email.Should().NotBeNullOrEmpty();
            employee.Phone.Should().NotBeNullOrEmpty();
            employee.CreatedAt.Date.Should().Be(DateTime.Now.Date);
        }
    }
}
