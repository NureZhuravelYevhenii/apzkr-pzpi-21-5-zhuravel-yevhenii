using VetAutoMobile.ApiLayer.Abstractions;
using VetAutoMobile.ApiLayer.Entities.Abstractions;
using VetAutoMobile.ApiLayer.Entities.AuthorizationConfigurations;
using VetAutoMobile.ApiLayer.Entities.EndpointConfigurations;

namespace VetAutoMobile.ApiLayer
{
    public class BaseEntityApiService<T, TId, TCreation, TUpdate> : BaseApiService, IEntityApiService<T, TId, TCreation, TUpdate>
    {
        private readonly IEntityEndpointConfiguration _endpointConfiguration;

        public BaseEntityApiService(
            AuthorizationConfiguration authorizationConfiguration,
            IHttpClientFabric httpClientFabric,
            BaseEntityEndpointConfiguration<T> endpointConfiguration) : base(httpClientFabric, authorizationConfiguration)
        {
            _endpointConfiguration = endpointConfiguration;
        }

        public Task CreateAsync(TCreation newEntity, CancellationToken cancellationToken = default)
            => SendRequest(newEntity, _endpointConfiguration.CreateEndpoint, HttpMethod.Post, cancellationToken);

        public Task DeleteAsync(TId deleteEntity, CancellationToken cancellationToken = default) 
            => SendRequest(deleteEntity, _endpointConfiguration.DeleteEndpoint, HttpMethod.Delete, cancellationToken);

        public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
            => SendRequest<IEnumerable<T>>(_endpointConfiguration.GetAllEndpoint, new List<T>(), cancellationToken: cancellationToken);

        public Task<T?> GetByIdAsync(TId entityId, CancellationToken cancellationToken = default)
            => SendRequest<T?>(
                _endpointConfiguration.GetByIdEndpoint,
                default,
                queryValues: typeof(TId).GetProperties().ToDictionary(property => property.Name, property => property.GetValue(entityId)?.ToString() ?? ""), 
                cancellationToken: cancellationToken);

        public Task UpdateAsync(TUpdate updateEntity, CancellationToken cancellationToken = default)
            => SendRequest(updateEntity, _endpointConfiguration.UpdateEndpoint, HttpMethod.Put, cancellationToken);
    }
}
