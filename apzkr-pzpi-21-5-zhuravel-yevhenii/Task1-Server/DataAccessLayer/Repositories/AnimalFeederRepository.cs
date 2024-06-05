using Core.Configurations;
using Core.Enums;
using Core.Localizations;
using DataAccessLayer.Abstractions.Repositories;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.BaseRepositories;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAccessLayer.Repositories
{
    public class AnimalFeederRepository : BaseRepository<AnimalFeeder>, IAnimalFeederRepository
    {
        public AnimalFeederRepository(IOptions<MongoDbConfiguration> mongoDbConfigurationOptions, IStringLocalizer<Resource> stringLocalizer) : base(mongoDbConfigurationOptions, stringLocalizer)
        {
        }

        public async Task<IEnumerable<Feeder>> GetFeedersByPopularityAsync(int take, CancellationToken cancellationToken = default)
        {
            return await _collection
                .Aggregate(
                    new EmptyPipelineDefinition<AnimalFeeder>()
                        .Group(af => af.FeederId, afg => new AnimalFeederGroup(afg.Key, afg.Count()))
                        .Sort(Builders<AnimalFeederGroup>.Sort.Descending("CountAnimals"))
                        .Limit(take)
                        .Lookup<AnimalFeeder, AnimalFeederGroup, Feeder, Feeder[]>(_database.GetCollection<Feeder>("feeders"), "FeederId", "_id", "feeders")
                        .Unwind<AnimalFeeder, Feeder[], AnimalFeederWithFeeder>("feeders")
                        .Project<AnimalFeeder, AnimalFeederWithFeeder, Feeder>(new BsonDocument()
                        {
                            {"_id", "$feeders._id" },
                            {"Location", "$feeders.Location" },
                            {"AnimalCenterId", "$feeders.AnimalCenterId" }
                        })
                )
                .ToListAsync(cancellationToken);
        }
        private readonly record struct AnimalFeederGroup(Guid FeederId, int CountAnimals);
        private record AnimalFeederWithFeeder(Guid FeederId, int CountAnimals, Feeder feeders);
    }
}
