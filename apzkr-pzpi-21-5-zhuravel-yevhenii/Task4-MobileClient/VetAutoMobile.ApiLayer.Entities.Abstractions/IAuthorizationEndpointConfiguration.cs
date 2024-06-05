namespace VetAutoMobile.ApiLayer.Entities.Abstractions
{
    public interface IAuthorizationEndpointConfiguration
    {
        string LoginEndpoint { get; }
        string RegistrationEndpoint { get; }
        string RefreshEndpoint { get; }
    }
}
