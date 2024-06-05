using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BusinessLogicLayer.Entities.AnimalFeeders
{
    public class AnimalFeederCreationDto
    {
        [Key]
        [JsonIgnore]
        public Guid Id { get; set; }
        public Guid AnimalId { get; set; }
        public Guid FeederId { get; set; }
        public double AmountOfFood { get; set; }
        [JsonIgnore]
        public DateTime FeedDate { get; set; }
    }
}
