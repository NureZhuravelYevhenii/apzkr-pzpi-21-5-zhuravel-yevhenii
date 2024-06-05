using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Entities.Animals
{
    public class AnimalUpdateDto
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid TypeId { get; set; }
        public Guid AnimalCenterId { get; set; }
    }
}
