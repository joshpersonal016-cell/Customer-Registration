using Customer.Registration.Application.Dtos;
using Customer.Registration.Domain.Entities;

namespace Customer.Registration.Application.Interface.Infrastructure.Repositories
{
    public interface ICustomerRepository
    {
        public Task CreateCustomerAsync(CustomerEntitiy customer);
        public Task<GetCustomerDto?> GetCustomerByIdAsync(Guid id);
        public Task<List<GetCustomerDto>> GetAllCustomerAsync();
    }
}
