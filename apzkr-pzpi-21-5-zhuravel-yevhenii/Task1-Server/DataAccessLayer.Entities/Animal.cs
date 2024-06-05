using DataAccessLayer.Entities.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities
{
    [CollectionName("animals")]
    public class Animal : ICloneable
    {
        [Key]
        [BsonId]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid TypeId { get; set; }
        public Guid AnimalCenterId { get; set; }

        [BsonIgnore]
        public AnimalCenter AnimalCenter { get; set; } = null!;
        [BsonIgnore]
        public IEnumerable<AnimalFeeder> AnimalFeeders { get; set; } = new List<AnimalFeeder>();
        [BsonIgnore]
        public AnimalType Type { get; set; } = null!;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
