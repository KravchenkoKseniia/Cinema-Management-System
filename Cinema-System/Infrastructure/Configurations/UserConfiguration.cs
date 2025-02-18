using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .HasColumnName("UserId")
                .ValueGeneratedOnAdd();

            // Map Identity-specific properties
            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.NormalizedUserName)
                .HasMaxLength(100);
            
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);
            builder.Property(u => u.NormalizedEmail)
                .HasMaxLength(150);
            builder.Property(u => u.EmailConfirmed)
                .HasDefaultValue(false);

            builder.Property(u => u.PasswordHash)
                .IsRequired();
            builder.Property(u => u.SecurityStamp);
            builder.Property(u => u.ConcurrencyStamp)
                .IsConcurrencyToken();
            builder.Property(u => u.PhoneNumber);
            builder.Property(u => u.PhoneNumberConfirmed)
                .HasDefaultValue(false);
            builder.Property(u => u.TwoFactorEnabled)
                .HasDefaultValue(false);
            builder.Property(u => u.LockoutEnd);
            builder.Property(u => u.LockoutEnabled)
                .HasDefaultValue(false);
            builder.Property(u => u.AccessFailedCount)
                .HasDefaultValue(0);

            // Map your custom properties
            /*builder.Property(u => u.RoleId).IsRequired();
            builder.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);*/
        }
    }
}