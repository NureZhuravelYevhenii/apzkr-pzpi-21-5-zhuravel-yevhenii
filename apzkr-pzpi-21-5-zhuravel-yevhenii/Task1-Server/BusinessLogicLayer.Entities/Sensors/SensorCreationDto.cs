using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Entities.Sensors
{
    public class SensorCreationDto
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AnimalId { get; set; }
        public Guid TypeId { get; set; }
    }
}
