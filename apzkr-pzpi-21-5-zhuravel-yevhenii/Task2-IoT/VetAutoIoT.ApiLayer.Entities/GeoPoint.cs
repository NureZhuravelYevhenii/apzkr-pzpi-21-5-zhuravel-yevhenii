namespace VetAutoIoT.ApiLayer.Entities
{
    public class GeoPoint
    {
        public IEnumerable<double> Coordinates { get; set; } = [];

        public GeoPoint(double latitude, double longitude)
        {
            Coordinates = [latitude, longitude];
        }
    }
}
