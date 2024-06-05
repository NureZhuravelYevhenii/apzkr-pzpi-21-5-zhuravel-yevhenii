using BusinessLogicLayer.Entities.Animals;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Entities.AnimalCenters
{
    public class AnimalCenterDto
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Info { get; set; } = string.Empty;
        public ICollection<AnimalDto> Animals { get; set; } = new List<AnimalDto>();
    }
}
