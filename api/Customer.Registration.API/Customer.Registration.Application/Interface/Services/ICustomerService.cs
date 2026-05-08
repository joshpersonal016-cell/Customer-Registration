using Customer.Registration.Application.Dtos;

namespace Customer.Registration.Application.Interface.Services
{
    public interface ICustomerService
    {
        Task CreateCustomerAsync(CreateCustomerDto dto);
        Task<GetCustomerDto?> GetCustomerByIdAsync(Guid id);
        Task<List<GetCustomerDto>> GetAllCustomerAsync();
    }
}
