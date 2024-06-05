using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFramework
{
    public class VetAutoContext : DbContext
    {
        public DbSet<Animal> Animals { get; set; }
        public DbSet<AnimalCenter> AnimalCenters { get; set; }
        public DbSet<AnimalFeeder> AnimalFeeders { get; set; }
        public DbSet<AnimalType> AnimalTypes { get; set; }
        public DbSet<Feeder> Feeders { get; set; }
        public DbSet<GeoPoint> GeoPoints { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<SensorType> SensorTypes { get; set; }

        public VetAutoContext(DbContextOptions<VetAutoContext> options) : base(options)
        {

        }

        public VetAutoContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=ZHEKA;Database=ThirdLabDb;Trusted_Connection=True;Encrypt=False;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VetAutoContext).Assembly);
        }
    }
}
