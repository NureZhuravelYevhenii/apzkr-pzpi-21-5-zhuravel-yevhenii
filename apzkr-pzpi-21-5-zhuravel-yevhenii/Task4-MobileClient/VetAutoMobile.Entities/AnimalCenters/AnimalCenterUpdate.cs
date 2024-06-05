namespace VetAutoMobile.Entities.AnimalCenters
{
    public class AnimalCenterUpdate
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Info { get; set; } = string.Empty;
    }
}
