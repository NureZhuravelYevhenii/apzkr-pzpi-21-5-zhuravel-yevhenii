using VetAutoMobile.Entities.Attributes;

namespace VetAutoMobile.Entities.Animals
{
    [EndpointName("Animals")]
    public class Animal
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid TypeId { get; set; }
        public Guid AnimalCenterId { get; set; }
    }
}
