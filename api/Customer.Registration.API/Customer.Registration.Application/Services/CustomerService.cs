using Customer.Registration.Application.Dtos;
using Customer.Registration.Application.Interface.Infrastructure.Repositories;
using Customer.Registration.Application.Interface.Services;
using Customer.Registration.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Customer.Registration.Application.Services
{
    public class CustomerService(
        ILogger<CustomerService> logger,
        ICustomerRepository repository) : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger = logger;
        private readonly ICustomerRepository _repository = repository;

        public async Task CreateCustomerAsync(CreateCustomerDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FirstName))
                throw new ArgumentException("FirstName is required");
            if (string.IsNullOrWhiteSpace(dto.LastName))
                throw new ArgumentException("LastName is required");
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email is required");
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
                throw new ArgumentException("PhoneNumber is required");

            var customer = new CustomerEntitiy
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("Creating customer with email: {Email}", dto.Email);

            await _repository.CreateCustomerAsync(customer);

            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("Customer created with email: {Email}", dto.Email);
        }

        public async Task<GetCustomerDto?> GetCustomerByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id is required");

            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("Getting customer with id: {Id}", id);
            return await _repository.GetCustomerByIdAsync(id) ?? throw new ArgumentException("Customer not found");
        }

        public async Task<List<GetCustomerDto>> GetAllCustomerAsync()
        {
            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("Getting all customers");
            return await _repository.GetAllCustomerAsync();
        }
    }
}
