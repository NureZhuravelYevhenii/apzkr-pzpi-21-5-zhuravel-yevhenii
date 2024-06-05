using VetAutoMobile.Entities.Attributes;

namespace VetAutoMobile.Entities.Feeders
{
    [EndpointName("Feeders")]
    public class Feeder
    {
        public Guid Id { get; set; }
        public GeoPoint Location { get; set; } = null!;
        public Guid AnimalCenterId { get; set; }
    }
}
