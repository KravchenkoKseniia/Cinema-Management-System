using CMS.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMS.Infrastructure.Data
{
    public static class SeedDataExtensions
    {
        public static void SeedRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "User" }
            );
        }
    } // replace with Microsoft.AspNetCore.Identity later 
}
