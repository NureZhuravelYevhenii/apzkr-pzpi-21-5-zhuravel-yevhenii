using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Entities.AnimalFeeders
{
    public class AnimalFeederUpdateDto
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AnimalId { get; set; }
        public Guid FeederId { get; set; }
        public DateTime FeedDate { get; set; }
        public double AmountOfFood { get; set; }
    }
}
