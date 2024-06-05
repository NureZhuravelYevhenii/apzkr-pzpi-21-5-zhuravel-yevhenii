using Microsoft.Extensions.Options;
using VetAutoMobile.ApiLayer.Abstractions;
using VetAutoMobile.ApiLayer.Entities.HttpClientConfigurations;

namespace VetAutoMobile.ApiLayer
{
    public class HttpClientFabric : IHttpClientFabric
    {
        private readonly HttpClientConfiguration _httpClientConfiguration;

        public HttpClientFabric(IOptions<HttpClientConfiguration> options)
        {
            _httpClientConfiguration = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public HttpClient CreateHttpClient()
        {
            return new HttpClient
            {
                BaseAddress = new Uri(_httpClientConfiguration.BaseUrl)
            };
        }
    }
}
