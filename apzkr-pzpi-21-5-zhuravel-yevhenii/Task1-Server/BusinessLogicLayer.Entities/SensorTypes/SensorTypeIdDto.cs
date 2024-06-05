using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Entities.SensorTypes
{
    public class SensorTypeIdDto
    {
        [Key]
        public Guid Id { get; set; }
    }
}
