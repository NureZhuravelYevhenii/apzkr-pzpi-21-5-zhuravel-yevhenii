using AutoMapper;
using BusinessLogicLayer.Abstractions;
using BusinessLogicLayer.Entities.AnimalFeeders;
using BusinessLogicLayer.Entities.Feeders;
using BusinessLogicLayer.Entities.Pagination;
using BusinessLogicLayer.Services.BaseServices;
using Core.Enums;
using Core.Localizations;
using Core.Services.TimeServices;
using DataAccessLayer.Abstractions;
using DataAccessLayer.Abstractions.Repositories;
using DataAccessLayer.Entities;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class AnimalFeederService : BaseCrudService<AnimalFeeder, AnimalFeederIdDto, AnimalFeederDto, AnimalFeederCreationDto, AnimalFeederUpdateDto, KeyAttribute>, IAnimalFeederService
    {
        public AnimalFeederService(IMapper mapper, IUnitOfWork unitOfWork, IStringLocalizer<Resource> stringLocalizer) : base(mapper, unitOfWork, stringLocalizer)
        {
        }

        public override Task<AnimalFeederDto> CreateEntityAsync(AnimalFeederCreationDto newEntity, CancellationToken cancellationToken = default)
        {
            newEntity.FeedDate = TimeService.GetNow();
            return base.CreateEntityAsync(newEntity, cancellationToken);
        }

        public async Task<double> GetAverageEatenFoodAmountAsync(Guid animalId, CancellationToken cancellationToken = default)
        {
            return (await ReadEntitiesByPredicateAsync(af => af.AnimalId == animalId, PaginationParameters.All(), cancellationToken)).Average(af => af.AmountOfFood);
        }

        public async Task<Month> GetMostPopularMonthForFeederAsync(Guid feederId, CancellationToken cancellationToken = default)
        {
            var animalFeeders = await ReadEntitiesByPredicateAsync(af => af.FeederId == feederId, PaginationParameters.All(), cancellationToken);

            var groupByDayOfWeek = animalFeeders
                .GroupBy(af => af.FeedDate.Month)
                .Max(new UniversalComparer());

            return (Month)(groupByDayOfWeek?.Key ?? (-1));
        }

        public async Task<DayOfWeek> GetMostPopularDayOfWeekForFeederAsync(Guid feederId, CancellationToken cancellationToken = default)
        {
            var animalFeeders = await ReadEntitiesByPredicateAsync(af => af.FeederId == feederId, PaginationParameters.All(), cancellationToken);

            var groupByDayOfWeek = animalFeeders
                .GroupBy(af => af.FeedDate.DayOfWeek)
                .Max(new UniversalComparer());

            return groupByDayOfWeek?.Key ?? (DayOfWeek)(-1);
        }

        public async Task<Season> GetMostPopularSeasonForFeederAsync(Guid feederId, CancellationToken cancellationToken = default)
        {
            var animalFeeders = await ReadEntitiesByPredicateAsync(af => af.FeederId == feederId, PaginationParameters.All(), cancellationToken);

            var groupBySeason = animalFeeders
                .GroupBy(af => af.FeedDate.Month)
                .Select(g => new MonthCount(g.Key, g.Count()))
                .Aggregate(new Dictionary<int, int>
                {
                    {(int)Season.Undefined, 0 },
                    {(int)Season.Winter, 0 },
                    {(int)Season.Spring, 0 },
                    {(int)Season.Summer, 0 },
                    {(int)Season.Autumn, 0 }
                }, (a, mc) =>
                {
                    a[mc.Month switch
                    {
                        int m when (m >= 1 && m < 3) || m == 12 => (int)Season.Winter,
                        int m when m >= 3 && m < 6 => (int)Season.Spring,
                        int m when m >= 6 && m < 9 => (int)Season.Summer,
                        int m when m >= 9 && m < 12 => (int)Season.Autumn,
                        _ => (int)Season.Undefined
                    }] += mc.Count;
                    return a;
                })
                .Max(new UniversalComparer());

            return ((Season)groupBySeason.Key);
        }

        public async Task<IEnumerable<FeederDto>> GetFeedersByPopularityAsync(CancellationToken cancellationToken = default)
        {
            var repository = await GetSpecificRepositoryAsync<IAnimalFeederRepository>();

            return _mapper.Map<IEnumerable<FeederDto>>(await repository.GetFeedersByPopularityAsync(10, cancellationToken));
        }

        private readonly record struct MonthCount(int Month, int Count);

        private class UniversalComparer : 
            IComparer<KeyValuePair<int, int>>,
            IComparer<IGrouping<DayOfWeek, AnimalFeederDto>>,
            IComparer<IGrouping<int, AnimalFeederDto>>
        {
            public int Compare(KeyValuePair<int, int> x, KeyValuePair<int, int> y)
            {
                return x.Value.CompareTo(y.Value);
            }

            public int Compare(IGrouping<DayOfWeek, AnimalFeederDto>? x, IGrouping<DayOfWeek, AnimalFeederDto>? y)
            {
                return x?.Count().CompareTo(y?.Count()) ?? -1;
            }

            public int Compare(IGrouping<int, AnimalFeederDto>? x, IGrouping<int, AnimalFeederDto>? y)
            {
                return x?.Count().CompareTo(y?.Count()) ?? -1;
            }
        }
    }
}
