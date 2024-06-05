namespace VetAutoMobile.Entities.Animals
{
    public class AnimalUpdate
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid TypeId { get; set; }
        public Guid AnimalCenterId { get; set; }
    }
}
