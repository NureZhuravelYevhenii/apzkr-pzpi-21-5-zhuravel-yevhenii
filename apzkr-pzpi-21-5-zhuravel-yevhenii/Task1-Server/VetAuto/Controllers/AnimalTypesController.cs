using BusinessLogicLayer.Abstractions;
using BusinessLogicLayer.Entities.AnimalTypes;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using VetAuto.Models;

namespace VetAuto.Controllers
{
    [Route("api/animal-types")]
    [ApiController]
    public class AnimalTypesController : BaseController<AnimalType, AnimalTypeDto, AnimalTypeIdDto, AnimalTypeCreationDto, AnimalTypeUpdateDto, AnimalTypeFilter>
    {
        private readonly IAnimalTypeService _animalTypeService;

        public AnimalTypesController(IAnimalTypeService animalTypeService) : base(animalTypeService)
        {
            _animalTypeService = animalTypeService;
        }

        [HttpGet("average-visited/{id}")]
        public async Task<IActionResult> GetAverageVisitedFeederCountAsync(Guid id, CancellationToken cancellationToken)
        {
            return Ok(await _animalTypeService.GetAverageVisitedFeederCountAsync(id, cancellationToken));
        }

        [HttpGet("feeders-visited-int-season/{id}")]
        public async Task<IActionResult> GetFeedersThatAnimalTypeVisitedInSeasonAsync(Guid id, CancellationToken cancellationToken)
        {
            return Ok(await _animalTypeService.GetFeedersThatAnimalTypeVisitedInSeasonAsync(id, cancellationToken));
        }

        public override Task<IActionResult> GetEntitiesAsync([FromQuery] AnimalTypeFilter animalTypeFilter, CancellationToken cancellationToken)
        {
            return GetEntitiesAsync(animalTypeFilter, at => at.Name.Contains(animalTypeFilter.Name), cancellationToken);
        }
    }
}
