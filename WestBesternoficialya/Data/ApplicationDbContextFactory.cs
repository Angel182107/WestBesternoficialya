using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WestBesternoficialya.Models;

namespace WestBesternoficialya.Data
{
    public class ApplicationDbContextFactory
        : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            var connectionString = "Server=localhost;Port=3306;Database=Hotel_San_Pedro;User=root;password=;";

            optionsBuilder.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString)
            );

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}