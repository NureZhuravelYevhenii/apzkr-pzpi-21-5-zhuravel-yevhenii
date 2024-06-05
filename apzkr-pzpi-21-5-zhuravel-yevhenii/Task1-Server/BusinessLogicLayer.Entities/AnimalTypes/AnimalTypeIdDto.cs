using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Entities.AnimalTypes
{
    public class AnimalTypeIdDto
    {
        [Key]
        public Guid Id { get; set; }
    }
}
