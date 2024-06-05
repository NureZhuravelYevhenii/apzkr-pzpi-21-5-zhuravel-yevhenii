using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EntityFramework.Configurations
{
    public class AnimalCenterConfiguration : IEntityTypeConfiguration<AnimalCenter>
    {
        public void Configure(EntityTypeBuilder<AnimalCenter> builder)
        {
            builder.HasMany(ac => ac.Animals).WithOne(a => a.AnimalCenter).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
