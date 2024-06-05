using AutoMapper;
using BusinessLogicLayer.Abstractions;
using BusinessLogicLayer.Entities.AnimalTypes;
using BusinessLogicLayer.Entities.Feeders;
using BusinessLogicLayer.Entities.Pagination;
using BusinessLogicLayer.Services.BaseServices;
using Core.Localizations;
using DataAccessLayer.Abstractions;
using DataAccessLayer.Abstractions.Repositories;
using DataAccessLayer.Entities;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Services
{
    public class AnimalTypeService : BaseCrudService<AnimalType, AnimalTypeIdDto, AnimalTypeDto, AnimalTypeCreationDto, AnimalTypeUpdateDto, KeyAttribute>, IAnimalTypeService
    {
        public AnimalTypeService(IMapper mapper, IUnitOfWork unitOfWork, IStringLocalizer<Resource> stringLocalizer) : base(mapper, unitOfWork, stringLocalizer)
        {
        }

        public async Task<int> GetAverageVisitedFeederCountAsync(Guid animalTypeId, CancellationToken cancellationToken = default)
        {
            var repository = await GetSpecificRepositoryAsync<IAnimalTypeRepository>(cancellationToken);

            return await repository.GetAverageVisitedFeederCountAsync(animalTypeId, cancellationToken);
        }

        public async Task<IEnumerable<FeederDto>> GetFeedersThatAnimalTypeVisitedInSeasonAsync(Guid animalTypeId, CancellationToken cancellationToken = default)
        {
            var repository = await GetSpecificRepositoryAsync<IAnimalTypeRepository>(cancellationToken);

            return _mapper.Map<IEnumerable<FeederDto>>(await repository.GetFeedersThatAnimalTypeVisitedInSeasonAsync(animalTypeId, cancellationToken));
        }
    }
}
