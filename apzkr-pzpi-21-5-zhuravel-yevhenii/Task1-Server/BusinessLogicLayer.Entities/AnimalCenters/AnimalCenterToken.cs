using System.Text.Json.Serialization;

namespace BusinessLogicLayer.Entities.AnimalCenters
{
    public class AnimalCenterToken
    {
        public string AccessToken { get; set; } = string.Empty;
        [JsonIgnore]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
