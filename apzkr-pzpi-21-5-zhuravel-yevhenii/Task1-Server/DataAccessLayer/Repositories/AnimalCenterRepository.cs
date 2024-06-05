using Core.Configurations;
using Core.EntityHelpers;
using Core.Localizations;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.BaseRepositories;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories
{
    public class AnimalCenterRepository : BaseRepository<AnimalCenter>
    {
        private readonly SensorRepository _sensorRepository;
        private readonly FeederRepository _feederRepository;
        private readonly AnimalRepository _animalRepository;

        public AnimalCenterRepository(IOptions<MongoDbConfiguration> mongoDbConfigurationOptions, IStringLocalizer<Resource> stringLocalizer) : base(mongoDbConfigurationOptions, stringLocalizer)
        {
            _sensorRepository = new SensorRepository(mongoDbConfigurationOptions, stringLocalizer);
            _feederRepository = new FeederRepository(mongoDbConfigurationOptions, stringLocalizer);
            _animalRepository = new AnimalRepository(mongoDbConfigurationOptions, stringLocalizer);
        }

        public override async Task<AnimalCenter?> ReadByIdAsync(EntityIds id, CancellationToken cancellationToken = default)
        {
            var filter = GetFilterDefinition(id);

            var viewProject = new BsonDocument
            {
                {"_id", "$_id" },
                {"Name", "$Name" },
                {"PasswordHash", "$PasswordHash" },
                {"RefreshToken", "$RefreshToken" },
                {"Address", "$Address" },
                {"Info", "$Info" },
                {"Animals", "$animals" }
            };

            await _database.CreateViewAsync(
                "animal-center-view",
                "animal-centers",
                new EmptyPipelineDefinition<AnimalCenter>()
                    .Lookup<AnimalCenter, AnimalCenter, Animal, Animal[]>(_database.GetCollection<Animal>("animals"), "_id", "AnimalCenterId", "animals")
                    .Project<AnimalCenter, Animal[], AnimalCenter>(viewProject)
                );

            var view = _database.GetCollection<AnimalCenter>("animal-center-view");

            return await view
                .Find(filter)
                .SortBy(ac => ac.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public override async Task<IEnumerable<AnimalCenter>> ReadByPredicateAsync(Expression<Func<AnimalCenter, bool>> predicate, int take, int skip, IDictionary<Expression<Func<AnimalCenter, object>>, bool> orderBy, CancellationToken cancellationToken = default)
        {
            var viewProject = new BsonDocument
            {
                {"_id", "$_id" },
                {"Name", "$Name" },
                {"PasswordHash", "$PasswordHash" },
                {"RefreshToken", "$RefreshToken" },
                {"Address", "$Address" },
                {"Info", "$Info" },
                {"Animals", "$animals" }
            };

            await _database.CreateViewAsync(
                "animal-center-view",
                "animal-centers",
                new EmptyPipelineDefinition<AnimalCenter>()
                    .Lookup<AnimalCenter, AnimalCenter, Animal, IEnumerable<Animal>>(_database.GetCollection<Animal>("animals"), "_id", "AnimalCenterId", "animals")
                    .Project<AnimalCenter, IEnumerable<Animal>, AnimalCenter>(viewProject)
                );

            var view = _database.GetCollection<AnimalCenter>("animal-center-view");

            return await view
                .Find(predicate)
                .SortBy(ac => ac.Id)
                .ToListAsync(cancellationToken);
        }

        public override async Task<bool> DeleteAsync(EntityIds id, CancellationToken cancellationToken = default)
        {
            var animalCenterFromId = GeneralIdExtractor<AnimalCenter, KeyAttribute>.MapEntityFromEntityIds(id);

            await _sensorRepository.DeleteSeveralAsync(s => s.AnimalCenterId == animalCenterFromId.Id, cancellationToken);
            await _feederRepository.DeleteSeveralAsync(s => s.AnimalCenterId == animalCenterFromId.Id, cancellationToken);
            await _animalRepository.DeleteSeveralAsync(s => s.AnimalCenterId == animalCenterFromId.Id, cancellationToken);

            return await  base.DeleteAsync(id, cancellationToken);
        }

        public override async Task<bool> DeleteSeveralAsync(Expression<Func<AnimalCenter, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var animalCenters = await ReadByPredicateAsync(predicate, int.MaxValue, 0, cancellationToken);

            foreach (var animalCenter in animalCenters)
            {
                await _sensorRepository.DeleteSeveralAsync(s => s.AnimalCenterId == animalCenter.Id, cancellationToken);
                await _feederRepository.DeleteSeveralAsync(s => s.AnimalCenterId == animalCenter.Id, cancellationToken);
                await _animalRepository.DeleteSeveralAsync(s => s.AnimalCenterId == animalCenter.Id, cancellationToken);
            }

            return await base.DeleteSeveralAsync(predicate, cancellationToken);
        }
    }
}
