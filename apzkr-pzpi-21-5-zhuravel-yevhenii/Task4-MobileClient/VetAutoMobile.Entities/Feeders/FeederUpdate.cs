namespace VetAutoMobile.Entities.Feeders
{
    public class FeederUpdate
    {
        public Guid Id { get; set; }
        public GeoPoint Location { get; set; } = null!;
        public Guid AnimalCenterId { get; set; }
    }

}
