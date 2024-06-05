using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Entities.Animals
{
    public class AnimalIdDto
    {
        [Key]
        public Guid Id { get; set; }
    }
}
