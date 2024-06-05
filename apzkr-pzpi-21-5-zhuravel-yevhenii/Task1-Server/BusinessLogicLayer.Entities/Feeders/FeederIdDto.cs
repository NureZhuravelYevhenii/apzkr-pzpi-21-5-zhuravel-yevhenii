using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Entities.Feeders
{
    public class FeederIdDto
    {
        [Key]
        public Guid Id { get; set; }
    }
}
