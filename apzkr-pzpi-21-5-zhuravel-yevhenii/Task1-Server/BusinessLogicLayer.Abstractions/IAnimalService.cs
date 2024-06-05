using BusinessLogicLayer.Abstractions.BaseInterfaces;
using BusinessLogicLayer.Entities.AnimalFeeders;
using BusinessLogicLayer.Entities.Animals;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Abstractions
{
    public interface IAnimalService : ICrudService<Animal, AnimalIdDto, AnimalDto, AnimalCreationDto, AnimalUpdateDto>
    {
        Task<int> GetTimeAnimalNotEatAsync(Guid animalId, CancellationToken cancellationToken = default);
        Task<IEnumerable<AnimalFeederDto>> GetAllAnimalFeedingsInPeriodAsync(Guid animalId, CancellationToken cancellationToken = default, DateTime? start = null, DateTime? finish = null);
        Task<IEnumerable<AnimalFeedingPlace>> GetAnimalFeedingPlaceAsync(Guid animalId, DateTime? start = null, DateTime? end = null, CancellationToken cancellationToken = default);
    }
}
