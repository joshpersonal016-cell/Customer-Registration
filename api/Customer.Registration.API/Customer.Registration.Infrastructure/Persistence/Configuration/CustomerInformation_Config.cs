using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Customer.Registration.Domain.Entities;

namespace Customer.Registration.Infrastructure.Persistence.Configuration
{
    public class CustomerInformation_Config : IEntityTypeConfiguration<Domain.Entities.CustomerEntitiy>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.CustomerEntitiy> builder)
        {
            builder.ToTable("CustomerInformations");

            // Primary Key (from BaseEntity)
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever(); // Generate Id in application, not database

            builder.Property(x => x.FirstName).HasColumnType("nvarchar").HasMaxLength(100);
            builder.Property(x => x.LastName).HasColumnType("nvarchar").HasMaxLength(100);
            builder.Property(x => x.Email).HasColumnType("nvarchar").HasMaxLength(100);
            builder.Property(x => x.PhoneNumber).HasColumnType("nvarchar").HasMaxLength(20);
            builder.Property(x => x.CreatedAt).HasColumnType("datetime2").IsRequired();
            builder.Property(x => x.UpdatedAt).HasColumnType("datetime2");
        }
    }
}
