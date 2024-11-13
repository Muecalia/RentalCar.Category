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
    public class UpdadeEmployeeHandlerTest
    {
        [Fact]
        public async void UpdadeEmployee_Executed_Return_string()
        {
            var employee = new Employee
            {
                Id = "Id",
                Email = "email",
                Gender = 'F',
                Name = "name",
                Phone = "123456789",
                CreatedAt = DateTime.Now
            };

            var updateEmployeeRequest = new UpdateEmployeeRequest 
            {
                Email = "email",
                Gender = 'F',
                Name = "name",
                Id = "Id",
                Phone = "1234567890"
            };

            var _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var _loggerServiceMock = new Mock<ILoggerService>();
            var _iRabbitMqServiceMock = new Mock<IRabbitMqService>();

            /*
            var accountRequest = new UpdateAccountRequest(employee.Id, employee.Name, employee.Email, employee.Phone);
                _iRabbitMqService.PublishMessage(accountRequest, RabbitQueue.UPDATE_ACCOUNT_QUEUE);
            */

            _employeeRepositoryMock.Setup(repo => repo.GetEmployee(It.IsAny<string>(), new CancellationToken())).ReturnsAsync(employee);
            _employeeRepositoryMock.Setup(repo => repo.Update(It.IsAny<Employee>(), new CancellationToken()));
            _iRabbitMqServiceMock.Setup(service => service.PublishMessage(It.IsAny<UpdateAccountRequest>(), It.IsAny<string>(), new CancellationToken()));

            var updadeEmployeeHandler = new UpdadeEmployeeHandler(_employeeRepositoryMock.Object, _loggerServiceMock.Object, _iRabbitMqServiceMock.Object);

            // Act
            var result = await updadeEmployeeHandler.Handle(updateEmployeeRequest, new CancellationToken());

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Message.Should().NotBeNullOrEmpty();
            result.Succeeded.Should().BeTrue();

            _employeeRepositoryMock.Verify(repo => repo.GetEmployee(It.IsAny<string>(), new CancellationToken()), Times.Once);
            _employeeRepositoryMock.Verify(repo => repo.Update(It.IsAny<Employee>(), new CancellationToken()), Times.Once);
        }
    }
}
