using DataAccessLayer.Entities.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities
{
    [CollectionName("feeders")]
    public class Feeder : ICloneable
    {
        [Key]
        [BsonId]
        public Guid Id { get; set; }
        public GeoPoint Location { get; set; } = null!;
        public Guid AnimalCenterId { get; set; }

        [BsonIgnore]
        public IEnumerable<AnimalFeeder> AnimalFeeders { get; set; } = new List<AnimalFeeder>();

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
