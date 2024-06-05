using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BusinessLogicLayer.Entities.Animals
{
    public class AnimalCreationDto
    {
        [JsonIgnore]
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public Guid TypeId { get; set; }
        public Guid AnimalCenterId { get; set; }
    }
}
