namespace BusinessLogicLayer.Entities.Animals
{
    public class AnimalFeedingPlace
    {
        public DateTime FeedingDate { get; set; }
        public ICollection<double> Coordinates { get; set; } = new List<double>();
    }
}
