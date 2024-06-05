using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VetAutoMobile.ApiLayer.Abstractions;
using VetAutoMobile.ApiLayer.Entities.Abstractions;
using VetAutoMobile.ApiLayer.Entities.EndpointConfigurations;
using VetAutoMobile.ApiLayer.Entities.HttpClientConfigurations;

namespace VetAutoMobile.ApiLayer
{
    public static class DependencyInjector
    {
        public static void Inject(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddScoped(typeof(IEntityApiService<,,,>), typeof(BaseEntityApiService<,,,>));
            serviceCollection.AddScoped<IAuthorizationApiService, AuthorizationApiService>();
            serviceCollection.AddScoped<IAnimalApiService, AnimalApiService>();
            serviceCollection.AddScoped<IHttpClientFabric, HttpClientFabric>();
            serviceCollection.AddScoped<IConfigurationService, ConfigurationService>();

            serviceCollection.AddScoped(typeof(BaseEntityEndpointConfiguration<>));
            serviceCollection.AddScoped<IAuthorizationEndpointConfiguration, AuthorizationEndpointConfiguration>();
            serviceCollection.Configure<HttpClientConfiguration>(httpClientConfiguration =>
            {
                httpClientConfiguration.BaseUrl = configuration["ApiConfiguration:BaseUrl"] ?? throw new ArgumentException("Unable to get api base url.");
            });
        }
    }
}
