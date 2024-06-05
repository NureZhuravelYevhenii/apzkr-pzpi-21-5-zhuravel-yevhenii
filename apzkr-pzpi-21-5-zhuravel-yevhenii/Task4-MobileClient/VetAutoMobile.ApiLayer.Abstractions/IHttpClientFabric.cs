namespace VetAutoMobile.ApiLayer.Abstractions
{
    public interface IHttpClientFabric
    {
        HttpClient CreateHttpClient();
    }
}
