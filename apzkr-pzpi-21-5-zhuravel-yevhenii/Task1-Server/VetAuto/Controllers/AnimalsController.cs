using BusinessLogicLayer.Abstractions;
using BusinessLogicLayer.Entities.AnimalFeeders;
using BusinessLogicLayer.Entities.Animals;
using Core.Constants;
using Core.Localizations;
using Core.Services.TimeServices;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using VetAuto.Models;

namespace VetAuto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : BaseController<Animal, AnimalDto, AnimalIdDto, AnimalCreationDto, AnimalUpdateDto, BaseFilter>
    {
        private readonly IAnimalService _animalService;
        private readonly IAnimalFeederService _animalFeederService;
        private readonly IStringLocalizer<Resource> _stringLocalizer;

        public AnimalsController(
            IAnimalService animalService,
            IAnimalFeederService animalFeederService,
            IStringLocalizer<Resource> stringLocalizer) : base(animalService) 
        {
            _animalService = animalService;
            _animalFeederService = animalFeederService;
            _stringLocalizer = stringLocalizer;
        }

        [HttpPost("create-animal-feeder")]
        public async Task<IActionResult> CreateAnimalFeederRelation(AnimalFeederCreationDto animalFeeder, CancellationToken cancellationToken)
        {
            animalFeeder.FeedDate = TimeService.GetNow();
            return Ok(await _animalFeederService.CreateEntityAsync(animalFeeder, cancellationToken));
        }

        [HttpGet("feeding/{id}")]
        public async Task<IActionResult> GetAnimalFeedingTimesAsync(Guid id, CancellationToken cancellationToken)
        {
            return Ok(await _animalService.GetAllAnimalFeedingsInPeriodAsync(id, cancellationToken));
        }

        [HttpGet("feeding-places/{id}")]
        public async Task<IActionResult> GetAnimalFeedingPlaceAsync([FromRoute]Guid id, [FromQuery]TimePeriod time, CancellationToken cancellationToken)
        {
            return Ok(await _animalService.GetAnimalFeedingPlaceAsync(id, time.Start, time.End, cancellationToken));
        }

        [HttpGet("no-eat/{id}")]
        public async Task<IActionResult> GetTimeAnimalNotEatAsync(Guid id, CancellationToken cancellationToken)
        {
            return Ok(await _animalService.GetTimeAnimalNotEatAsync(id, cancellationToken));
        }

        [HttpGet("average-eaten-food/{id}")]
        public async Task<IActionResult> GetAverageEatenFoodAsync(Guid id, CancellationToken cancellationToken)
        {
            return Ok(await _animalFeederService.GetAverageEatenFoodAmountAsync(id, cancellationToken));
        }

        public override Task<IActionResult> CreateNewEntityAsync(AnimalCreationDto entity, CancellationToken cancellationToken)
        {
            var id = User.FindFirstValue(AnimalCenterConstants.Id)
                ?? throw new ArgumentException(_stringLocalizer["There is no registered Animal Center id in token."].Value);

            entity.AnimalCenterId = new Guid(id);

            return base.CreateNewEntityAsync(entity, cancellationToken);
        }
    }
}
