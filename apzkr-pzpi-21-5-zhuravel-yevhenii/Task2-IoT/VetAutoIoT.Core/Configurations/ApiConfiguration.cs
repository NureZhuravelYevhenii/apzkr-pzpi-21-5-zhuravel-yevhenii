namespace VetAutoIoT.Core.Configurations
{
    public class ApiConfiguration
    {
        public int Id { get; set; }
        public string BaseUrl { get; set; } = string.Empty;
        public string AnimalFeederEndpoint { get; set; } = string.Empty;
        public string FeederByCoordinatesEndpoint { get; set; } = string.Empty;
        public string FeederEndpoint { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;

        public void SetPrototypeValues(ApiConfiguration prototype)
        {
            BaseUrl = prototype.BaseUrl;
            AnimalFeederEndpoint = prototype.AnimalFeederEndpoint;
            FeederByCoordinatesEndpoint = prototype.FeederByCoordinatesEndpoint;
            FeederEndpoint = prototype.FeederEndpoint;
            AccessToken = prototype.AccessToken;
        }
    }
}
