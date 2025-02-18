using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // Use the inherited Id as the key.
            builder.HasKey(r => r.Id);

            // Configure Name property (required for Identity)
            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(50);

            // (Optionally) configure NormalizedName and ConcurrencyStamp if needed.
        }
    }
}