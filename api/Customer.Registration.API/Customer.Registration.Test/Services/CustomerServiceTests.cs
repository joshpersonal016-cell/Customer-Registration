using Customer.Registration.Application.Dtos;
using Customer.Registration.Application.Interface.Infrastructure.Repositories;
using Customer.Registration.Application.Services;
using Customer.Registration.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Customer.Registration.Test.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _repoMock;
        private readonly Mock<ILogger<CustomerService>> _loggerMock;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            _repoMock = new Mock<ICustomerRepository>();
            _loggerMock = new Mock<ILogger<CustomerService>>();

            _service = new CustomerService(
                _loggerMock.Object,
                _repoMock.Object
            );
        }

        // Create customer tests

        [Fact]
        public async Task CreateCustomerAsync_ShouldThrow_WhenFirstNameIsEmpty()
        {
            var dto = ValidDto();
            dto.FirstName = "";

            Func<Task> act = async () => await _service.CreateCustomerAsync(dto);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("FirstName is required");
        }

        [Fact]
        public async Task CreateCustomerAsync_ShouldThrow_WhenLastNameIsEmpty()
        {
            var dto = ValidDto();
            dto.LastName = "";

            Func<Task> act = async () => await _service.CreateCustomerAsync(dto);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("LastName is required");
        }

        [Fact]
        public async Task CreateCustomerAsync_ShouldThrow_WhenEmailIsEmpty()
        {
            var dto = ValidDto();
            dto.Email = "";

            Func<Task> act = async () => await _service.CreateCustomerAsync(dto);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Email is required");
        }

        [Fact]
        public async Task CreateCustomerAsync_ShouldThrow_WhenPhoneNumberIsEmpty()
        {
            var dto = ValidDto();
            dto.PhoneNumber = "";

            Func<Task> act = async () => await _service.CreateCustomerAsync(dto);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("PhoneNumber is required");
        }

        [Fact]
        public async Task CreateCustomerAsync_ShouldCallRepository_WhenValid()
        {
            var dto = ValidDto();

            _repoMock
                .Setup(x => x.CreateCustomerAsync(It.IsAny<CustomerEntitiy>()))
                .Returns(Task.CompletedTask);

            await _service.CreateCustomerAsync(dto);

            _repoMock.Verify(
                x => x.CreateCustomerAsync(It.IsAny<CustomerEntitiy>()),
                Times.Once
            );
        }


        // Get customer by id tests

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldThrow_WhenIdIsEmpty()
        {
            Func<Task> act = async () =>
                await _service.GetCustomerByIdAsync(Guid.Empty);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Id is required");
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldThrow_WhenNotExist()
        {
            var id = Guid.NewGuid();

            _repoMock
                .Setup(x => x.GetCustomerByIdAsync(id))
                .ReturnsAsync((GetCustomerDto?)null);

            Func<Task> act = async () =>
                await _service.GetCustomerByIdAsync(id);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Customer not found");
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldReturnCustomer_WhenExist()
        {
            var id = Guid.NewGuid();

            var expected = new GetCustomerDto
            {
                Id = id,
                FirstName = "Joshua",
                LastName = "Aguilar",
                Email = "jaguilar@test.com",
                PhoneNumber = "09123456789",
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            };

            _repoMock
                .Setup(x => x.GetCustomerByIdAsync(id))
                .ReturnsAsync(expected);

            var result = await _service.GetCustomerByIdAsync(id);

            result.Should().BeEquivalentTo(expected);
        }


        // Get all customers tests
        [Fact]
        public async Task GetAllCustomerAsync_ShouldReturnList()
        {
            var list = new List<GetCustomerDto>
            {
                new() { 
                    FirstName = "Joshua",
                    LastName = "Aguilar",
                    Email = "jaguilar@test.com",
                    PhoneNumber = "09123456789"
                },
                new() {
                    FirstName = "Joshua",
                    LastName = "Aguilar",
                    Email = "jaguilar@test.com",
                    PhoneNumber = "09123456789"
                }
            };

            _repoMock
                .Setup(x => x.GetAllCustomerAsync())
                .ReturnsAsync(list);

            var result = await _service.GetAllCustomerAsync();

            result.Should().HaveCount(2);
        }


        // Helpers
        private static CreateCustomerDto ValidDto()
        {
            return new CreateCustomerDto
            {
                FirstName = "Joshua",
                LastName = "Aguilar",
                Email = "jaguilar@test.com",
                PhoneNumber = "09123456789"
            };
        }
    }
}
