using Core.Configurations;
using Core.EntityHelpers;
using Core.Enums;
using Core.Localizations;
using Core.Services.TimeServices;
using DataAccessLayer.Abstractions.Repositories;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.BaseRepositories;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories
{
    public class AnimalRepository : BaseRepository<Animal>, IAnimalRepository
    {
        private readonly AnimalFeederRepository _animalFeederRepository;
        private readonly FeederRepository _feederRepository;

        public AnimalRepository(IOptions<MongoDbConfiguration> mongoDbConfigurationOptions, IStringLocalizer<Resource> stringLocalizer) : base(mongoDbConfigurationOptions, stringLocalizer)
        {
            _animalFeederRepository = new AnimalFeederRepository(mongoDbConfigurationOptions, stringLocalizer);
            _feederRepository = new FeederRepository(mongoDbConfigurationOptions, stringLocalizer);
        }

        public async Task<int> GetNumberOfVisitedFeedersAsync(Guid animalId, CancellationToken cancellationToken = default)
        {
            var animalFeeders = await _animalFeederRepository.ReadByPredicateAsync(af => af.AnimalId == animalId, int.MaxValue, 0, cancellationToken);

            return animalFeeders.Distinct(new AnimalFeederComparer()).Count();
        }

        public async Task<IEnumerable<Feeder>> GetVisitedFeederInSeasonAsync(Guid animalId, Season season = Season.Undefined, CancellationToken cancellationToken = default)
        {
            var month = TimeService.GetNow().Month;

            season = season == Season.Undefined ? month switch
            {
                int m when m == 12 || m < 3 => Season.Winter,
                int m when m > 2 || m < 6 => Season.Spring,
                int m when m > 5 || m < 9 => Season.Summer,
                int m when m > 8 || m < 12 => Season.Autumn,
                _ => Season.Undefined
            } : season;

            var dates = GetSeasonStartEndDates(season);

            var animalFeeders = await _animalFeederRepository.ReadByPredicateAsync(
                af => af.AnimalId == animalId && af.FeedDate > dates.StartDate && af.FeedDate < dates.EndDate,
                int.MaxValue, 
                0,
                cancellationToken);
            var feeders = new List<Feeder>();
            foreach (var animalFeeder in animalFeeders)
            {
                var feederEntity = await _feederRepository.ReadByIdAsync(new EntityIds
                {
                    {"Id", animalFeeder.FeederId}
                });
                if (feederEntity is not null)
                {
                    feeders.Add(feederEntity);
                }
            }

            return feeders.Distinct();
        }

        //&&
        //af.FeedDate > dates.StartDate &&
        //        af.FeedDate<dates.EndDate

        public override async Task<bool> DeleteAsync(EntityIds id, CancellationToken cancellationToken = default)
        {
            var animalFromId = GeneralIdExtractor<Animal, KeyAttribute>.MapEntityFromEntityIds(id);

            await _animalFeederRepository.DeleteSeveralAsync(af => af.AnimalId == animalFromId.Id, cancellationToken);

            return await base.DeleteAsync(id, cancellationToken);
        }

        public override async Task<bool> DeleteSeveralAsync(Expression<Func<Animal, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var animals = await ReadByPredicateAsync(predicate, int.MaxValue, 0, cancellationToken);

            foreach(var animal in animals)
            {
                await _animalFeederRepository.DeleteSeveralAsync(af => af.AnimalId == animal.Id, cancellationToken);
            }

            return await base.DeleteSeveralAsync(predicate, cancellationToken);
        }

        public override Task<Animal> CreateAsync(Animal entity, string localizationCode, CancellationToken cancellationToken = default)
        {
            return base.CreateAsync(entity, localizationCode, cancellationToken);
        }

        private (DateTime StartDate, DateTime EndDate) GetSeasonStartEndDates(Season season)
        {
            var now = TimeService.GetNow();

            var months = season switch
            {
                Season.Winter => (12, 1, 2),
                Season.Spring => (-1, 3, 5),
                Season.Summer => (-1, 6, 8),
                Season.Autumn => (-1, 9, 11),
                _ => (-1, -1, -1)
            };

            if (months.Item3 < now.Month)
            {
                if (season == Season.Winter)
                {
                    return (new DateTime(now.Year - 2, months.Item1, 1).ToUniversalTime(), new DateTime(now.Year - 1, months.Item3 + 1, 1).ToUniversalTime());
                }
                return (new DateTime(now.Year - 1, months.Item2, 1).ToUniversalTime(), new DateTime(now.Year - 1, months.Item3 + 1, 1).ToUniversalTime());
            }

            if (season == Season.Winter)
            {
                return (new DateTime(now.Year - 1, months.Item1, 1).ToUniversalTime(), new DateTime(now.Year, months.Item3 + 1, 1).ToUniversalTime());
            }
            return (new DateTime(now.Year, months.Item2, 1).ToUniversalTime(), new DateTime(now.Year, months.Item3 + 1, 1).ToUniversalTime());
        }

        private class AnimalFeederComparer : IEqualityComparer<AnimalFeeder>
        {
            public bool Equals(AnimalFeeder? x, AnimalFeeder? y)
            {
                return x?.FeederId.Equals(y?.FeederId) ?? false;
            }

            public int GetHashCode([DisallowNull] AnimalFeeder obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
