using DataAccessLayer.Entities.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities
{
    [CollectionName("animal-centers")]
    public class AnimalCenter : ICloneable
    {
        [Key]
        [BsonId]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string RefreshToken { get;set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Info { get; set; } = string.Empty;

        public ICollection<Animal> Animals { get; set; } = new List<Animal>();

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
