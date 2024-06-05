using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Entities.GeoPoints
{
    public class GeoPointIdDto
    {

        [Key]
        public Guid FeederId { get; set; }
    }
}
