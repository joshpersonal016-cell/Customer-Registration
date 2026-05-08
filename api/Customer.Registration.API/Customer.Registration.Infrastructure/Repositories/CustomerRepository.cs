using Customer.Registration.Application.Dtos;
using Customer.Registration.Application.Interface.Infrastructure.Repositories;
using Customer.Registration.Domain.Entities;
using Customer.Registration.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace Customer.Registration.Infrastructure.Repositories
{
    public class CustomerRepository(CustomerDBContext context) : ICustomerRepository
    {
        private readonly CustomerDBContext _context = context;

        public async Task CreateCustomerAsync(CustomerEntitiy customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<GetCustomerDto?> GetCustomerByIdAsync(Guid id)
        {
            return await _context.Customers
                .AsNoTracking()
                .Where(x => x.Id == id)
                .ProjectToType<GetCustomerDto>()
                .FirstOrDefaultAsync();
        }

        public async Task<List<GetCustomerDto>> GetAllCustomerAsync()
        {
            return await _context.Customers
                .AsNoTracking()
                .ProjectToType<GetCustomerDto>()
                .ToListAsync();
        }
    }
}
