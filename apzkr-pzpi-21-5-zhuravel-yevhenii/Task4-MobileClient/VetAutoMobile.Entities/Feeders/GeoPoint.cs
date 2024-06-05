namespace VetAutoMobile.Entities.Feeders
{
    public class GeoPoint
    {
        public double[] Coordinates { get; set; } = [];

        public override string ToString()
        {
            return string.Join(", ", Coordinates);
        }

        public static implicit operator GeoPoint(string coordinates) => new GeoPoint
        {
            Coordinates = coordinates.Split(", ").Select(double.Parse).ToArray(),
        };
    }
}
