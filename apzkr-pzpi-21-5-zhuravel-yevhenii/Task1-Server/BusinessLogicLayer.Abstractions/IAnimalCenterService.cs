using BusinessLogicLayer.Abstractions.BaseInterfaces;
using BusinessLogicLayer.Entities.AnimalCenters;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Abstractions
{
    public interface IAnimalCenterService : ICrudService<AnimalCenter, AnimalCenterIdDto, AnimalCenterDto, AnimalCenterCreationDto, AnimalCenterUpdateDto>
    {
        Task<AnimalCenterToken> LoginAsync(string login, string password, CancellationToken cancellationToken = default);
        Task<AnimalCenterToken> LoginAsync(AnimalCenterToken tokens, CancellationToken cancellationToken = default);
        Task<AnimalCenterToken> RegisterAsync(AnimalCenterCreationDto animalCenter, CancellationToken cancellationToken = default);
    }
}
