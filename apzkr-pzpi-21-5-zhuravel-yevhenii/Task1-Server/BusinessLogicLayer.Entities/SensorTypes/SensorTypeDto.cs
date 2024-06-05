using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Entities.SensorTypes
{
    public class SensorTypeDto
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
