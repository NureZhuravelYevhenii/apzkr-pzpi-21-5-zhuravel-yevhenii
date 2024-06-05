using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Entities.Animals
{
    public class AnimalDto
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid TypeId { get; set; }
        public Guid AnimalCenterId { get; set; }
    }
}
