using VetAutoMobile.ApiLayer.Abstractions;
using VetAutoMobile.ApiLayer.Entities.AuthorizationConfigurations;
using VetAutoMobile.Entities.Animals;

namespace VetAutoMobile.ApiLayer
{
    public class AnimalApiService : BaseApiService, IAnimalApiService
    {
        public AnimalApiService(IHttpClientFabric httpClientFabric, AuthorizationConfiguration authorizationConfiguration) : base(httpClientFabric, authorizationConfiguration)
        {
        }

        public Task<IEnumerable<AnimalFeedingPlace>> GetFeedingPlacesAsync(Guid animalId)
        {
            return SendRequest<IEnumerable<AnimalFeedingPlace>>($"animals/feeding-places/{animalId}", new List<AnimalFeedingPlace>());
        }
    }
}
