using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    public class GeoPoint
    {
        [Key]
        public int Id { get; set; }

        [BsonIgnore]
        public Guid FeederId { get; set; }
        [BsonIgnore]
        public Feeder Feeder { get; set; } = null!;

        [BsonElement("coordinates")]
        public ICollection<double> Coordinates { get; set; } = new List<double>();

        [BsonElement("type")]
        [NotMapped]
        public string Type { get; set; } = "Point";
    }
}
