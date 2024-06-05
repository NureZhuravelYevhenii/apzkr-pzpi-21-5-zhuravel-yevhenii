using Core.Configurations;
using Core.EntityHelpers;
using Core.Localizations;
using DataAccessLayer.Abstractions.Repositories;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.BaseRepositories;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories
{
    public class AnimalTypeRepository : BaseRepository<AnimalType>, IAnimalTypeRepository
    {
        private readonly AnimalRepository _animalRepository;

        public AnimalTypeRepository(IOptions<MongoDbConfiguration> mongoDbConfigurationOptions, IStringLocalizer<Resource> stringLocalizer) : base(mongoDbConfigurationOptions, stringLocalizer)
        {
            _animalRepository = new AnimalRepository(mongoDbConfigurationOptions, stringLocalizer);
        }

        public async Task<int> GetAverageVisitedFeederCountAsync(Guid animalTypeId, CancellationToken cancellationToken = default)
        {
            var animalsOfType = await _animalRepository.ReadByPredicateAsync(a => a.TypeId == animalTypeId, int.MaxValue, 0, cancellationToken);

            var visitedFeedersTasks = animalsOfType.Select(async a => await _animalRepository.GetNumberOfVisitedFeedersAsync(a.Id, cancellationToken));

            await Task.WhenAll(visitedFeedersTasks);

            return (int)Math.Ceiling(visitedFeedersTasks.Select(t => t.Result).Average());
        }

        public async Task<IEnumerable<Feeder>> GetFeedersThatAnimalTypeVisitedInSeasonAsync(Guid animalTypeId, CancellationToken cancellationToken = default)
        {
            var animals = await _animalRepository.ReadByPredicateAsync(a => a.TypeId == animalTypeId, int.MaxValue, 0, cancellationToken);
            var feeders = new List<Feeder>();

            foreach (var animal in animals)
            {
                feeders.AddRange(await _animalRepository.GetVisitedFeederInSeasonAsync(animal.Id, cancellationToken: cancellationToken));
            }

            return feeders.Distinct();
        }

        public override async Task<bool> DeleteAsync(EntityIds id, CancellationToken cancellationToken = default)
        {
            var animalType = GeneralIdExtractor<AnimalType, KeyAttribute>.MapEntityFromEntityIds(id);

            await _animalRepository.DeleteSeveralAsync(a => a.TypeId == animalType.Id, cancellationToken);

            return await base.DeleteAsync(id, cancellationToken);
        }

        public override async Task<bool> DeleteSeveralAsync(Expression<Func<AnimalType, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var animalTypes = await ReadByPredicateAsync(predicate, int.MaxValue, 0, cancellationToken);

            foreach (var animalType in animalTypes)
            {
                await _animalRepository.DeleteSeveralAsync(a => a.TypeId == animalType.Id, cancellationToken);
            }

            return await base.DeleteSeveralAsync(predicate, cancellationToken);
        }
    }
}
