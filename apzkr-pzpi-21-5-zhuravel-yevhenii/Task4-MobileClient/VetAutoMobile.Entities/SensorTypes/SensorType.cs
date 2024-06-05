using VetAutoMobile.Entities.Attributes;

namespace VetAutoMobile.Entities.SensorTypes
{
    [EndpointName("sensor-types")]
    public class SensorType
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

}
