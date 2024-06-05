using AutoMapper;
using BusinessLogicLayer.Abstractions;
using BusinessLogicLayer.Entities.AnimalFeeders;
using BusinessLogicLayer.Entities.Animals;
using BusinessLogicLayer.Entities.Feeders;
using BusinessLogicLayer.Entities.Pagination;
using BusinessLogicLayer.Services.BaseServices;
using Core.Localizations;
using Core.Services.TimeServices;
using DataAccessLayer.Abstractions;
using DataAccessLayer.Entities;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace BusinessLogicLayer.Services
{
    public class AnimalService : BaseCrudService<Animal, AnimalIdDto, AnimalDto, AnimalCreationDto, AnimalUpdateDto, KeyAttribute>, IAnimalService
    {
        private readonly IAnimalFeederService _animalFeederService;
        private readonly IFeederService _feederService;

        public AnimalService(
            IAnimalFeederService animalFeederService,
            IFeederService feederService,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IStringLocalizer<Resource> stringLocalizer
            ) : base(mapper, unitOfWork, stringLocalizer)
        {
            _animalFeederService = animalFeederService;
            _feederService = feederService;
        }

        public async Task<int> GetTimeAnimalNotEatAsync(Guid animalId, CancellationToken cancellationToken = default)
        {
            var animalFeeders = await _animalFeederService.ReadEntitiesByPredicateAsync(
                af => af.AnimalId == animalId,
                PaginationParameters.All(),
                new Dictionary<Expression<Func<AnimalFeeder, object>>, bool>
                {
                    {af => af.FeedDate, true }
                },
                 cancellationToken);

            if (animalFeeders.Any())
            {
                var latestFeeding = animalFeeders.First();

                return (TimeService.GetNow() - latestFeeding.FeedDate.ToUniversalTime()).Hours;
            }

            return new TimeSpan(0, 0, 0).Hours;
        }

        public async Task<IEnumerable<AnimalFeederDto>> GetAllAnimalFeedingsInPeriodAsync(Guid animalId, CancellationToken cancellationToken = default, DateTime? start = null, DateTime? finish = null)
        {
            start ??= TimeService.GetMinimum();
            finish ??= TimeService.GetMaximum();

            var animalFeeders = await _animalFeederService.ReadEntitiesByPredicateAsync(
                af => af.AnimalId == animalId &&
                af.FeedDate > start &&
                af.FeedDate < finish,
                PaginationParameters.All(),
                cancellationToken
                );

            return animalFeeders;
        }

        public async Task<IEnumerable<AnimalFeedingPlace>> GetAnimalFeedingPlaceAsync(Guid animalId, DateTime? start = null, DateTime? end = null, CancellationToken cancellationToken = default)
        {
            start ??= TimeService.GetMinimum();
            end ??= TimeService.GetMaximum();
            var animalFeeders = await _animalFeederService.ReadEntitiesByPredicateAsync(
                af => af.AnimalId == animalId && af.FeedDate > start && af.FeedDate < end,
                PaginationParameters.All(),
                cancellationToken
                );

            var feedingPlaces = new List<AnimalFeedingPlace>();
            foreach (var animalFeeder in animalFeeders)
            {
                var feeder = await _feederService.ReadEntityByIdAsync(new FeederIdDto
                {
                    Id = animalFeeder.FeederId,
                },
                cancellationToken);

                if (feeder is not null)
                {
                    feedingPlaces.Add(new AnimalFeedingPlace
                    {
                        Coordinates = feeder.Location.Coordinates,
                        FeedingDate = animalFeeder.FeedDate,
                    });
                }
            }

            return feedingPlaces;
        }
    }
}
