using FluentAssertions;
using Moq;
using RentalCar.Employees.Application.Commands.Request.Account;
using RentalCar.Employees.Application.Commands.Request.Employee;
using RentalCar.Employees.Application.Handlers.Employees;
using RentalCar.Employees.Core.Entities;
using RentalCar.Employees.Core.Repositories;
using RentalCar.Employees.Core.Services;

namespace RentalCar.Employees.UnitTests.Application.Commands.Employees
{
    public class CreateEmployeeHandlerTest
    {
        [Fact]
        public async void CreateEmployee_Executed_Return_string()
        {
            // Arrange 
            var createEmployeeRequest = new CreateEmployeeRequest 
            {
                Name = "name",
                Email = "email",
                Password = "password",
                Gender = 'F',
                Phone = "123456789",
                Role = "role"
            };

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

            _employeeRepositoryMock.Setup(repo => repo.IsEmployeeExist(It.IsAny<string>(), new CancellationToken())).ReturnsAsync(false);
            _employeeRepositoryMock.Setup(repo => repo.IsEmailExist(It.IsAny<string>(), new CancellationToken())).ReturnsAsync(false);
            _employeeRepositoryMock.Setup(repo => repo.Create(It.IsAny<Employee>(), new CancellationToken())).ReturnsAsync(employee);
            _iRabbitMqServiceMock.Setup(service => service.PublishMessage(It.IsAny<CreateAccountRequest>(), It.IsAny<string>(), new CancellationToken()));

            var createEmployeeHandler = new CreateEmployeeHandler(_employeeRepositoryMock.Object, _loggerServiceMock.Object, _iRabbitMqServiceMock.Object);

            // Act
            var result = await createEmployeeHandler.Handle(createEmployeeRequest, new CancellationToken());

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Message.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();

            _employeeRepositoryMock.Verify(repo => repo.IsEmployeeExist(It.IsAny<string>(), new CancellationToken()), Times.Once());
            _employeeRepositoryMock.Verify(repo => repo.IsEmailExist(It.IsAny<string>(), new CancellationToken()), Times.Once());
            _employeeRepositoryMock.Verify(repo => repo.Create(It.IsAny<Employee>(), new CancellationToken()), Times.Once());

        }
    }
}
