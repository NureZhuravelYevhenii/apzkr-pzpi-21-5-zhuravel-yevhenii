using DataAccessLayer.Entities.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities
{
    [CollectionName("sensors")]
    public class Sensor : ICloneable
    {
        [Key]
        [BsonId]
        public Guid Id { get; set; }
        public Guid AnimalId { get; set; }
        public Guid TypeId { get; set; }
        public Guid AnimalCenterId { get; set; }

        [BsonIgnore]
        public Animal Animal { get; set; } = null!;
        [BsonIgnore]
        public SensorType SensorType { get; set; } = null!;
        [BsonIgnore]
        public AnimalCenter AnimalCenter { get; set; } = null!;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
