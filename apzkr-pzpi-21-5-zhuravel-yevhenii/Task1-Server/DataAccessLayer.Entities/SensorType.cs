using DataAccessLayer.Entities.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities
{
    [CollectionName("sensor-types")]
    public class SensorType : ICloneable
    {
        [Key]
        [BsonId]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        [BsonIgnore]
        public IEnumerable<Sensor> Sensors { get; set; } = new List<Sensor>();

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
