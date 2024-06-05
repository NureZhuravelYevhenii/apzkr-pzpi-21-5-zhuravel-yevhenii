using VetAutoMobile.Entities.Animals;

namespace VetAutoMobile.ApiLayer.Abstractions
{
    public interface IAnimalApiService
    {
        Task<IEnumerable<AnimalFeedingPlace>> GetFeedingPlacesAsync(Guid animalId);
    }
}
