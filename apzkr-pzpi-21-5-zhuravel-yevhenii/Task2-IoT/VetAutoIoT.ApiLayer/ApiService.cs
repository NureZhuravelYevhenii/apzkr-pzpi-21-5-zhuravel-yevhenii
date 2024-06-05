using System.Net.Http.Json;
using VetAutoIoT.ApiLayer.Abstractions;
using VetAutoIoT.ApiLayer.Entities;
using VetAutoIoT.Core.Abstractions;
using VetAutoIoT.Core.Configurations;

namespace VetAutoIoT.ApiLayer
{
    public class ApiService : IApiService
    {
        private const string NotConfiguredExceptionMessage = "Seams like you not configured application. Please run configure.";

        private readonly ApiConfiguration _apiConfiguration;
        private readonly FeederConfiguration _feederConfiguration;
        private readonly IApiHttpClientFactory _apiHttpClientFactory;

        public ApiService(
            IApiHttpClientFactory apiHttpClientFactory,
            ApiConfiguration apiConfiguration,
            FeederConfiguration feederConfiguration)
        {
            _apiConfiguration = apiConfiguration;
            _feederConfiguration = feederConfiguration;
            _apiHttpClientFactory = apiHttpClientFactory;
        }

        public Task CreateAnimalFeederAsync(AnimalFeederCreationDto animalFeeder, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(_apiConfiguration.BaseUrl) || string.IsNullOrEmpty(_apiConfiguration.AccessToken))
            {
                throw new ArgumentException(NotConfiguredExceptionMessage);
            }

            var client = _apiHttpClientFactory.CreateHttpClient();
            return client.PostAsync(_apiConfiguration.AnimalFeederEndpoint, JsonContent.Create(animalFeeder), cancellationToken);
        }

        public Task CreateFeederAsync(double latitude, double longitude, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(_apiConfiguration.BaseUrl) || string.IsNullOrEmpty(_apiConfiguration.AccessToken))
            {
                throw new ArgumentException(NotConfiguredExceptionMessage);
            }

            var client = _apiHttpClientFactory.CreateHttpClient();
            return client.PostAsync(_apiConfiguration.AnimalFeederEndpoint, JsonContent.Create(
                new GeoPoint(latitude, longitude)
                ), cancellationToken);
        }

        public async Task<FeederDto?> GetFeederDtoAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(_apiConfiguration.BaseUrl) || string.IsNullOrEmpty(_apiConfiguration.AccessToken))
            {
                throw new ArgumentException(NotConfiguredExceptionMessage);
            }

            var client = _apiHttpClientFactory.CreateHttpClient();
            var feeders = await client.GetFromJsonAsync<IEnumerable<FeederDto>>($"{_apiConfiguration.FeederByCoordinatesEndpoint}?coordinates={_feederConfiguration.Latitude} {_feederConfiguration.Longitude}", cancellationToken);
            return feeders?.FirstOrDefault();
        }
    }
}
