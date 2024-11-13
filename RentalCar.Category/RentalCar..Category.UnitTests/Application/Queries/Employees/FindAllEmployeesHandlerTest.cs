using FluentAssertions;
using Moq;
using RentalCar.Employees.Application.Handlers.Employees;
using RentalCar.Employees.Application.Queries.Request.Client;
using RentalCar.Employees.Core.Entities;
using RentalCar.Employees.Core.Repositories;
using RentalCar.Employees.Core.Services;

namespace RentalCar.Employees.UnitTests.Application.Queries.Employees
{
    public class FindAllEmployeesHandlerTest
    {
        [Fact]
        public async void FindAllEmployees_Executed_Return_List_FindAllEmployeesResponse()
        {
            // Arrange
            var employees = new List<Employee> 
            {
                new Employee { Id = "Id 1", Email = "email 1", Gender = 'F', Name = "Name 1", Phone = "123456789",  CreatedAt = DateTime.Now },
                new Employee { Id = "Id 2", Email = "email 2", Gender = 'M', Name = "Name 2", Phone = "123456789",  CreatedAt = DateTime.Now },
                new Employee { Id = "Id 3", Email = "email 3", Gender = 'F', Name = "Name 3", Phone = "123456789",  CreatedAt = DateTime.Now },
                new Employee { Id = "Id 4", Email = "email 4", Gender = 'M', Name = "Name 4", Phone = "123456789",  CreatedAt = DateTime.Now }
            };

            var _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var _loggerServiceMock = new Mock<ILoggerService>();

            _employeeRepositoryMock.Setup(repo => repo.GetAllEmployees(new CancellationToken())).ReturnsAsync(employees);

            var findAllEmployeesHandler = new FindAllEmployeesHandler(_employeeRepositoryMock.Object, _loggerServiceMock.Object);

            // Act
            var result = await findAllEmployeesHandler.Handle(new FindAllEmployeesRequest(), new CancellationToken());

            // Assert
            result.Datas.Should().NotBeNullOrEmpty();
            result.Datas.Should().HaveCount(employees.Count);
            result.Message.Should().NotBeNullOrEmpty();
            result.Succeeded.Should().BeTrue();

            _employeeRepositoryMock.Verify(repo => repo.GetAllEmployees(new CancellationToken()), Times.Once);
        }
    }
}
