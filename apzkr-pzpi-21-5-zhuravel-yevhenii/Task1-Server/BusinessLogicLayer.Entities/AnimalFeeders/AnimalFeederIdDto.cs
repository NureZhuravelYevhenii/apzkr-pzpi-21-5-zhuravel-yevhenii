using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Entities.AnimalFeeders
{
    public class AnimalFeederIdDto
    {
        [Key]
        public Guid Id { get; set; }
    }
}
