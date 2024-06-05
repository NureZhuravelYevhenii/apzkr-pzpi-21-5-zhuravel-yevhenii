namespace VetAutoIoT.ApiLayer.Entities
{
    public class AnimalFeederCreationDto
    {
        public Guid AnimalId { get; set; }
        public Guid FeederId { get; set; }
        public double AmountOfFood { get; set; }
    }
}
