using DataAccessLayer.Entities;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Entities.Feeders
{
    public class FeederDto
    {
        [Key]
        public Guid Id { get; set; }
        public GeoPoint Location { get; set; } = null!;
        public Guid AnimalCenterId { get; set; }
    }
}
