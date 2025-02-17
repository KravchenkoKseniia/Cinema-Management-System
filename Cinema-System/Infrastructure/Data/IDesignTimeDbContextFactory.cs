using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Infrastructure.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Start from the Infrastructure folder
            // Move one directory up, then into "MVC"
            var current = Directory.GetCurrentDirectory();
            var mvcFolder = Path.Combine(current, "..", "MVC");

            // If that still doesn't exist, try one more level up
            if (!Directory.Exists(Path.Combine(mvcFolder)))
            {
                mvcFolder = Path.Combine(current, "..", "..", "MVC");
            }

            // Now load appsettings.json from the MVC folder
            var configuration = new ConfigurationBuilder()
                .SetBasePath(mvcFolder)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}