namespace BusinessLogicLayer.Entities.GeoPoints
{
    public class GeoPointCreationDto
    {
        public ICollection<double> Coordinates { get; set; } = new List<double>();
        public string Type { get; set; } = "Point";
    }
}
