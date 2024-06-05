namespace VetAutoIoT.Core.Abstractions
{
    public interface IApiHttpClientFactory
    {
        public HttpClient CreateHttpClient();
    }
}
