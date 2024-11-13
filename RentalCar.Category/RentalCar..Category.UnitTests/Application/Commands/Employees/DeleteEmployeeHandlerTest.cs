using FluentAssertions;
using Moq;
using RentalCar.Employees.Application.Commands.Request.Employee;
using RentalCar.Employees.Application.Handlers.Employees;
using RentalCar.Employees.Core.Entities;
using RentalCar.Employees.Core.Repositories;
using RentalCar.Employees.Core.Services;

namespace RentalCar.Employees.UnitTests.Application.Commands.Employees
{
    public class DeleteEmployeeHandlerTest
    {
        [Fact]
        public async void DeleteEmployee_Executed_Return_InputEmployeeResponse()
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

            _employeeRepositoryMock.Setup(repo => repo.GetEmployee(It.IsAny<string>(), new CancellationToken())).ReturnsAsync(employee);
            _employeeRepositoryMock.Setup(repo => repo.Delete(It.IsAny<Employee>(), new CancellationToken()));

            var deleteEmployeeHandler = new DeleteEmployeeHandler(_employeeRepositoryMock.Object, _loggerServiceMock.Object, _iRabbitMqServiceMock.Object);

            // Act
            var result = await deleteEmployeeHandler.Handle(new DeleteEmployeeRequest(It.IsAny<string>()), new CancellationToken());

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Message.Should().NotBeNullOrEmpty();
            result.Succeeded.Should().BeTrue();

            _employeeRepositoryMock.Verify(repo => repo.GetEmployee(It.IsAny<string>(), new CancellationToken()), Times.Once);
            _employeeRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Employee>(), new CancellationToken()), Times.Once);
        }
    }
}
