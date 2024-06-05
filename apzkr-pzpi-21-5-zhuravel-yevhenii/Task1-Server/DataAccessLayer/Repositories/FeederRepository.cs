using Core.Configurations;
using Core.EntityHelpers;
using Core.Localizations;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.BaseRepositories;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories
{
    public class FeederRepository : BaseRepository<Feeder>
    {
        private readonly AnimalFeederRepository _animalFeederRepository;

        public FeederRepository(IOptions<MongoDbConfiguration> mongoDbConfigurationOptions, IStringLocalizer<Resource> stringLocalizer) : base(mongoDbConfigurationOptions, stringLocalizer)
        {
            _animalFeederRepository = new AnimalFeederRepository(mongoDbConfigurationOptions, stringLocalizer);
        }

        public override async Task<bool> DeleteAsync(EntityIds id, CancellationToken cancellationToken = default)
        {
            var feeder = GeneralIdExtractor<Feeder, KeyAttribute>.MapEntityFromEntityIds(id);

            await _animalFeederRepository.DeleteSeveralAsync(af => af.FeederId == feeder.Id, cancellationToken);

            return await base.DeleteAsync(id, cancellationToken);
        }

        public override async Task<bool> DeleteSeveralAsync(Expression<Func<Feeder, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var feeders = await ReadByPredicateAsync(predicate, int.MaxValue, 0, cancellationToken);

            foreach (var feeder in feeders)
            {
                await _animalFeederRepository.DeleteSeveralAsync(af => af.FeederId == feeder.Id, cancellationToken);
            }

            return await base.DeleteSeveralAsync(predicate, cancellationToken);
        }
    }
}
