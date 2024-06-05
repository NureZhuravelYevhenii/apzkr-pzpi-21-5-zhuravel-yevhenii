using System.Reflection;
using VetAutoMobile.ApiLayer.Entities.Abstractions;
using VetAutoMobile.Entities.Attributes;

namespace VetAutoMobile.ApiLayer.Entities.EndpointConfigurations
{
    public class BaseEntityEndpointConfiguration<T> : IEntityEndpointConfiguration
    {
        private readonly string _endpointName;

        public BaseEntityEndpointConfiguration()
        {
            _endpointName = typeof(T).GetCustomAttribute<EndpointNameAttribute>()?.EndpointName ?? typeof(T).Name;
        }

        public string GetByIdEndpoint => $"{_endpointName}/single";

        public string GetAllEndpoint => _endpointName;

        public string CreateEndpoint => _endpointName;

        public string DeleteEndpoint => _endpointName;

        public string UpdateEndpoint => _endpointName;
    }
}
