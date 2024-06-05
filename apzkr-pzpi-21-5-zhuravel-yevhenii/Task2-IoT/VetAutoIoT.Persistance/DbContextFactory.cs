using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace VetAutoIoT.Persistence
{
    public class DbContextFactory : IDesignTimeDbContextFactory<PersistenceDbContext>
    {
        public PersistenceDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();

            var options = new DbContextOptionsBuilder<PersistenceDbContext>()
                .UseSqlite(string.Format(configuration.GetConnectionString("SqliteConnectionString")!, Environment.CurrentDirectory))
                .Options;

            return new PersistenceDbContext(options);
        }
    }
}
