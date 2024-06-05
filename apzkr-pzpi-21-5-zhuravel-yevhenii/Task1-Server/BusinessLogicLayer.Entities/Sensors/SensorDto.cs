using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Entities.Sensors
{
    public class SensorDto
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AnimalId { get; set; }
        public Guid TypeId { get; set; }
    }
}
