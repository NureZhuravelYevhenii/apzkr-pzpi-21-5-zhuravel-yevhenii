using VetAutoIoT.ApiLayer.Entities;

namespace VetAutoIoT.ApiLayer.Abstractions
{
    public interface IApiService
    {
        Task CreateAnimalFeederAsync(AnimalFeederCreationDto animalFeeder, CancellationToken cancellationToken = default);
        Task<FeederDto?> GetFeederDtoAsync(CancellationToken cancellationToken = default);
        Task CreateFeederAsync(double latitude, double longitude, CancellationToken cancellationToken = default);
    }
}
