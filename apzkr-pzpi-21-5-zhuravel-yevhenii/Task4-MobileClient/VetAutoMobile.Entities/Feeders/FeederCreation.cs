namespace VetAutoMobile.Entities.Feeders
{
    public class FeederCreation
    {
        public Guid AnimalCenterId { get; set; }
        public GeoPoint Location { get; set; } = null!;
    }
}
