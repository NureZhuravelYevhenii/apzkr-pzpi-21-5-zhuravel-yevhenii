using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Entities.AnimalCenters
{
    public class AnimalCenterUpdateDto
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? PasswordHash { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Info { get; set; } = string.Empty;
    }
}
