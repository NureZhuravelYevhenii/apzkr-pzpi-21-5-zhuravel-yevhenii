using VetAutoMobile.ApiLayer.Entities.Abstractions;

namespace VetAutoMobile.ApiLayer.Entities.EndpointConfigurations
{
    public class AuthorizationEndpointConfiguration : IAuthorizationEndpointConfiguration
    {
        public string LoginEndpoint => "AnimalCenter/login";

        public string RegistrationEndpoint => "AnimalCenter/register";

        public string RefreshEndpoint => "AnimalCenter/refresh";
    }
}
