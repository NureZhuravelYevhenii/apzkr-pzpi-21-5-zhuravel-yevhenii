using BusinessLogicLayer.Entities.GeoPoints;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BusinessLogicLayer.Entities.Feeders
{
    public class FeederCreationDto
    {
        [Key]
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        public GeoPointCreationDto Location { get; set; } = null!;
        public Guid AnimalCenterId { get; set; }
    }
}
