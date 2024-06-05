using VetAutoIoT.Core.Abstractions;
using VetAutoIoT.Core.Configurations;

namespace VetAutoIoT.Core.HttpClient
{
    public class HttpClientFactory : IApiHttpClientFactory
    {
        private readonly ApiConfiguration _apiConfiguration;

        public HttpClientFactory(ApiConfiguration apiConfiguration)
        {
            _apiConfiguration = apiConfiguration;
        }

        public System.Net.Http.HttpClient CreateHttpClient()
        {
            return new System.Net.Http.HttpClient
            {
                BaseAddress = new Uri(_apiConfiguration.BaseUrl),
            };
        }
    }
}
