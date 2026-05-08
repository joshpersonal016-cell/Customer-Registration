using Customer.Registration.Application.Interface.Infrastructure.Repositories;
using Customer.Registration.Application.Interface.Services;
using Customer.Registration.Application.Services;
using Customer.Registration.Infrastructure.Persistence;
using Customer.Registration.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Customer.Registration.Infrastructure
{
    public static class DI
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddDbContext<CustomerDBContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("CustomerDB")));

            // Services DI
            services.AddScoped<ICustomerService, CustomerService>();

            // Repositories DI
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            

            return services;
        }
    }
}
