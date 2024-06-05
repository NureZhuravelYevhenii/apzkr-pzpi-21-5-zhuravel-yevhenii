using VetAutoMobile.Entities.Attributes;

namespace VetAutoMobile.Entities.Sensors
{
    [EndpointName("Sensors")]
    public class Sensor
    {
        public Guid Id { get; set; }
        public Guid AnimalId { get; set; }
        public Guid TypeId { get; set; }
    }

}
