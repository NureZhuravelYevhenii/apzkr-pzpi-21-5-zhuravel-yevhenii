using BusinessLogicLayer.Abstractions.BaseInterfaces;
using BusinessLogicLayer.Entities.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using VetAuto.Models;

namespace VetAuto.Controllers
{
    [ApiController]
    public class BaseController<T, TDto, TIdDto, TCreationDto, TUpdateDto, TFilter> : ControllerBase
        where TFilter: BaseFilter
    {
        private readonly ICrudService<T, TIdDto, TDto, TCreationDto, TUpdateDto> _crudService;

        public BaseController(ICrudService<T, TIdDto, TDto, TCreationDto, TUpdateDto> crudService)
        {
            _crudService = crudService;
        }

        [Authorize]
        [HttpPost]
        public virtual async Task<IActionResult> CreateNewEntityAsync(TCreationDto entity, CancellationToken cancellationToken)
        {
            var newEntity = await _crudService.CreateEntityAsync(entity, cancellationToken);

            return Ok(newEntity);
        }

        [Authorize]
        [HttpGet]
        public virtual async Task<IActionResult> GetEntitiesAsync([FromQuery] TFilter animalTypeFilter, CancellationToken cancellationToken)
        {
            var orderBys = Request.Query.Keys.ToDictionary(k => k, k => Request.Query[k].ToString());

            return Ok(await _crudService.ReadEntitiesByPredicateAsync(
                e => true,
                new PaginationParameters(animalTypeFilter.Page, animalTypeFilter.PageCount == 0 ? 10 : animalTypeFilter.PageCount),
                orderBys,
                cancellationToken));
        }

        [Authorize]
        [HttpGet("single")]
        public virtual async Task<IActionResult> GetEntityById([FromQuery] TIdDto idDto, CancellationToken cancellationToken)
        {
            return Ok(await _crudService.ReadEntityByIdAsync(idDto, cancellationToken));
        }

        [Authorize]
        [HttpDelete]
        public virtual async Task<IActionResult> DeleteAsync(TIdDto idDto, CancellationToken cancellationToken)
        {
            await _crudService.DeleteEntityAsync(idDto, cancellationToken);
            return Ok();
        }

        [Authorize]
        [HttpPut]
        public virtual async Task<IActionResult> UpdateAsync(TUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var updatedEntity = await _crudService.UpdateEntityAsync(updateDto, cancellationToken);
            return Ok(updatedEntity);
        }

        protected async Task<IActionResult> GetEntitiesAsync(BaseFilter filter, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            var orderBys = Request.Query.Keys.ToDictionary(k => k, k => Request.Query[k].ToString());

            return Ok(await _crudService.ReadEntitiesByPredicateAsync(
                predicate,
                new PaginationParameters(filter.Page, filter.PageCount == 0 ? 10 : filter.PageCount),
                orderBys,
                cancellationToken));
        }
    }
}
