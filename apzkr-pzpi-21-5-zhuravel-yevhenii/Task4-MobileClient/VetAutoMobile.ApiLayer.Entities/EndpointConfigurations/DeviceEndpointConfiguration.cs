namespace VetAutoMobile.ApiLayer.Entities.EndpointConfigurations
{
    public class DeviceEndpointConfiguration
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string AnimalFeederEndpoint { get; set; } = string.Empty;
        public string FeederByCoordinatesEndpoint { get; set; } = string.Empty;
        public string FeederEndpoint { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
    }
}
