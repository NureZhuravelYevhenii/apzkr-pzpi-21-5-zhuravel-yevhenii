using Microsoft.EntityFrameworkCore;
using VetAutoIoT.Core.Configurations;

namespace VetAutoIoT.Persistence
{
    public class PersistenceDbContext : DbContext
    {
        public DbSet<ApiConfiguration> ApiConfigurations { get; set; } = null!;
        public DbSet<FeederConfiguration> FeederConfigurations { get; set; } = null!;

        public PersistenceDbContext(DbContextOptions<PersistenceDbContext> options) : base(options)
        {

        }
    }
}
