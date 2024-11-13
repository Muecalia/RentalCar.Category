using FluentAssertions;
using Moq;
using RentalCar.Employees.Application.Handlers.Employees;
using RentalCar.Employees.Application.Queries.Request.Employee;
using RentalCar.Employees.Core.Entities;
using RentalCar.Employees.Core.Repositories;
using RentalCar.Employees.Core.Services;

namespace RentalCar.Employees.UnitTests.Application.Queries.Employees
{
    public class FindEmployeeByIdHandlerTest
    {
        [Fact]
        public async void FindEmployeeById_Executed_Return_FindEmployeeByIdResponse()
        {
            // Arrange
            var employee = new Employee
            {
                Id = "Id",
                Email = "email",
                Gender = 'F',
                Name = "name",
                Phone = "123456789",
                CreatedAt = DateTime.Now
            };

            var _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var _loggerServiceMock = new Mock<ILoggerService>();
            var _iRabbitMqServiceMock = new Mock<IRabbitMqService>();

            _employeeRepositoryMock.Setup(repo => repo.GetEmployeeDetail(It.IsAny<string>(), new CancellationToken())).ReturnsAsync(employee);

            var findEmployeeByIdHandler = new FindEmployeeByIdHandler(_employeeRepositoryMock.Object, _loggerServiceMock.Object, _iRabbitMqServiceMock.Object);

            // Act
            var result = await findEmployeeByIdHandler.Handle(new FindEmployeeByIdRequest(It.IsAny<string>()), new CancellationToken());

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Message.Should().NotBeNullOrEmpty();
            result.Succeeded.Should().BeTrue();

            _employeeRepositoryMock.Verify(repo => repo.GetEmployeeDetail(It.IsAny<string>(), new CancellationToken()), Times.Once);
        }
    }
}
