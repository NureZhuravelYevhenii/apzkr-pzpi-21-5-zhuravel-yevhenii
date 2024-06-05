using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Entities.AnimalCenters
{
    public class AnimalCenterIdDto
    {
        [Key]
        public Guid Id { get; set; }

        public AnimalCenterIdDto(Guid id)
        {
            Id = id;
        }

        public AnimalCenterIdDto()
        {

        }
    }
}
