using Customer.Registration.Domain.Entities.Common;

namespace Customer.Registration.Domain.Entities
{
    public class CustomerEntitiy : BaseEntity
    {
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public string? Email { get; set; } = null;
        public string? PhoneNumber { get; set; } = null;
    }
}
