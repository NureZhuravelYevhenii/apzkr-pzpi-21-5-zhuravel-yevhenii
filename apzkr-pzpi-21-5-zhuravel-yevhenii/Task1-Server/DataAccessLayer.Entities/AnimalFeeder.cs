using DataAccessLayer.Entities.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities
{
    [CollectionName("animals-feeders")]
    public class AnimalFeeder : ICloneable
    {
        [BsonId]
        [Key]
        public Guid Id { get; set; }

        public Guid AnimalId { get; set; }
        public Guid FeederId { get; set; }
        public double AmountOfFood { get; set; }

        private DateTime _feedDate;
        public DateTime FeedDate 
        {
            get => _feedDate.ToUniversalTime();
            set => _feedDate = value;
        }

        [BsonIgnore]
        public Feeder Feeder { get; set; } = null!;
        [BsonIgnore]
        public Animal Animal { get; set; } = null!;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
