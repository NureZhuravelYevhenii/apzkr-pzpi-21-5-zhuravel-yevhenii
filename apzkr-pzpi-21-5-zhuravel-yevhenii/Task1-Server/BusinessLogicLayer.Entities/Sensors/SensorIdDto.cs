using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Entities.Sensors
{
    public class SensorIdDto
    {
        [Key]
        public Guid Id { get; set; }
    }
}
