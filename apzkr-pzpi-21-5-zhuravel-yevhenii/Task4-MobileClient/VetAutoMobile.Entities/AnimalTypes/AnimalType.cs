using VetAutoMobile.Entities.Attributes;

namespace VetAutoMobile.Entities.AnimalTypes
{
    [EndpointName("animal-types")]
    public class AnimalType
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

}
