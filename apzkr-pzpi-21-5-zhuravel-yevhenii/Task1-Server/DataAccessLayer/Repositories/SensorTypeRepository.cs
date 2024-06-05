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
    public class SensorTypeRepository : BaseRepository<SensorType>
    {
        private readonly SensorRepository _sensorRepository;

        public SensorTypeRepository(IOptions<MongoDbConfiguration> mongoDbConfigurationOptions, IStringLocalizer<Resource> stringLocalizer) : base(mongoDbConfigurationOptions, stringLocalizer)
        {
            _sensorRepository = new SensorRepository(mongoDbConfigurationOptions, stringLocalizer);
        }

        public override async Task<bool> DeleteAsync(EntityIds id, CancellationToken cancellationToken = default)
        {
            var sensorType = GeneralIdExtractor<SensorType, KeyAttribute>.MapEntityFromEntityIds(id);

            await _sensorRepository.DeleteSeveralAsync(s => s.TypeId == sensorType.Id, cancellationToken);

            return await base.DeleteAsync(id, cancellationToken);
        }

        public override async Task<bool> DeleteSeveralAsync(Expression<Func<SensorType, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var sensorTypes = await ReadByPredicateAsync(predicate, int.MaxValue, 0, cancellationToken);

            foreach (var sensorType in sensorTypes)
            {
                await _sensorRepository.DeleteSeveralAsync(s => s.TypeId == sensorType.Id, cancellationToken);
            }

            return await base.DeleteSeveralAsync(predicate, cancellationToken);
        }
    }
}
