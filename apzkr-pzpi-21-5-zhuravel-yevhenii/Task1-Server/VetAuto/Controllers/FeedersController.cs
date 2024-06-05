using BusinessLogicLayer.Abstractions;
using BusinessLogicLayer.Abstractions.BaseInterfaces;
using BusinessLogicLayer.Entities.Feeders;
using BusinessLogicLayer.Entities.Pagination;
using Core.Constants;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VetAuto.Models;

namespace VetAuto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedersController : BaseController<Feeder, FeederDto, FeederIdDto, FeederCreationDto, FeederUpdateDto, BaseFilter>
    {
        private readonly IFeederService _crudService;
        private readonly IAnimalFeederService _animalFeederService;

        public FeedersController(IFeederService crudService, IAnimalFeederService animalFeederService) : base(crudService)
        {
            _crudService = crudService;
            _animalFeederService = animalFeederService;
        }

        public override Task<IActionResult> CreateNewEntityAsync(FeederCreationDto entity, CancellationToken cancellationToken)
        {
            var id = User.FindFirstValue(AnimalCenterConstants.Id)
                ?? throw new ArgumentException("There is no registered Animal Center id in token.");

            entity.AnimalCenterId = new Guid(id);

            return base.CreateNewEntityAsync(entity, cancellationToken);
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetFeedersByPopularityAsync(CancellationToken cancellationToken)
        {
            return Ok(await _animalFeederService.GetFeedersByPopularityAsync(cancellationToken));
        }

        [HttpGet("{id}/popular-season")]
        public async Task<IActionResult> GetMostPopularSeasonForFeederAsync(Guid id, CancellationToken cancellationToken)
        {
            return Ok((await _animalFeederService.GetMostPopularSeasonForFeederAsync(id, cancellationToken)).ToString());
        }

        [HttpGet("{id}/popular-month")]
        public async Task<IActionResult> GetMostPopularMonthForFeederAsync(Guid id, CancellationToken cancellationToken)
        {
            return Ok((await _animalFeederService.GetMostPopularMonthForFeederAsync(id, cancellationToken)).ToString());
        }

        [HttpGet("{id}/popular-day-of-week")]
        public async Task<IActionResult> GetMostPopularDayOfWeekForFeederAsync(Guid id, CancellationToken cancellationToken)
        {
            return Ok((await _animalFeederService.GetMostPopularDayOfWeekForFeederAsync(id, cancellationToken)).ToString());
        }

        [HttpGet("by-coordinates")]
        public async Task<IActionResult> GetFeedersByCoordinatesAsync([FromQuery] string coordinates, CancellationToken cancellationToken)
        {
            var feeders = await _crudService.ReadAllEntitiesAsync(cancellationToken);

            return Ok(feeders.Where(f => string.Join(' ', f.Location.Coordinates) == coordinates));
        }
    }
}
