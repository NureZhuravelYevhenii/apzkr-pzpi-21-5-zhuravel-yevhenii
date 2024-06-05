using DataAccessLayer.Entities;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Entities.Feeders
{
    public class FeederUpdateDto
    {
        [Key]
        public Guid Id { get; set; }
        public GeoPoint Location { get; set; } = null!;
        public Guid AnimalCenterId { get; set; }
    }
}
