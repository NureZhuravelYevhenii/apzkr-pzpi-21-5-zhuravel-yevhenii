using BusinessLogicLayer.Abstractions.BaseInterfaces;
using BusinessLogicLayer.Entities.AnimalTypes;
using BusinessLogicLayer.Entities.Feeders;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Abstractions
{
    public interface IAnimalTypeService : ICrudService<AnimalType, AnimalTypeIdDto, AnimalTypeDto, AnimalTypeCreationDto, AnimalTypeUpdateDto>
    {
        Task<int> GetAverageVisitedFeederCountAsync(Guid animalTypeId, CancellationToken cancellationToken = default);
        Task<IEnumerable<FeederDto>> GetFeedersThatAnimalTypeVisitedInSeasonAsync(Guid animalTypeId, CancellationToken cancellationToken = default);
    }
}
