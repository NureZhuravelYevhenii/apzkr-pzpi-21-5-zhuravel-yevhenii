using AutoMapper;
using BusinessLogicLayer.Abstractions.BaseInterfaces;
using BusinessLogicLayer.Entities.Pagination;
using BusinessLogicLayer.Helpers;
using Core.Constants;
using Core.EntityHelpers;
using Core.Localizations;
using DataAccessLayer.Abstractions;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

namespace BusinessLogicLayer.Services.BaseServices
{
    public class BaseCrudService<T, TIdDto, TDetailedDto, TCreateDto, TUpdateDto, TIdAttribute> : ICrudService<T, TIdDto, TDetailedDto, TCreateDto, TUpdateDto>
        where TIdAttribute : Attribute
        where T : class, ICloneable
    {
        protected readonly IMapper _mapper;
        protected readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<Resource> _stringLocalizer;
        private IRepository<T, Expression<Func<T, bool>>>? _repository;
        private readonly Dictionary<Type, object> _specificRepositories = new Dictionary<Type, object>();

        public BaseCrudService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IStringLocalizer<Resource> stringLocalizer
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _stringLocalizer = stringLocalizer;
        }

        public virtual async Task<TDetailedDto> CreateEntityAsync(TCreateDto newEntity, CancellationToken cancellationToken = default)
        {
            var createdEntity = await (await GetRepositoryAsync(cancellationToken))
                .CreateAsync(_mapper.Map<T>(newEntity), cancellationToken);
            return _mapper.Map<TDetailedDto>(createdEntity);
        }

        public virtual async Task DeleteEntityAsync(TIdDto ids, CancellationToken cancellationToken = default)
        {
            await (await GetRepositoryAsync()).DeleteAsync(GeneralIdExtractor<TIdDto, TIdAttribute>.GetId(ids), cancellationToken);
        }

        public virtual async Task<IEnumerable<TDetailedDto>> ReadEntitiesByPredicateAsync(Expression<Func<T, bool>> predicate, PaginationParameters paginationParameters, CancellationToken cancellationToken = default)
        {
            var fetchedEntities = await (await GetRepositoryAsync(cancellationToken))
                .ReadByPredicateAsync(predicate, paginationParameters.GetTake(), paginationParameters.GetSkip(), cancellationToken);

            return _mapper.Map<IEnumerable<TDetailedDto>>(
                fetchedEntities
            );
        }

        public virtual async Task<TDetailedDto?> ReadEntityByIdAsync(TIdDto ids, CancellationToken cancellationToken = default)
        {
            return _mapper.Map<TDetailedDto?>(
                await (await GetRepositoryAsync(cancellationToken))
                .ReadByIdAsync(GeneralIdExtractor<TIdDto, TIdAttribute>.GetId(ids), cancellationToken));
        }

        public virtual async Task<TDetailedDto> UpdateEntityAsync(TUpdateDto newEntity, CancellationToken cancellationToken = default)
        {
            var id = GeneralIdExtractor<TUpdateDto, TIdAttribute>.GetId(newEntity);
            var oldEntity = await (await GetRepositoryAsync(cancellationToken)).ReadByIdAsync(id, cancellationToken);

            if (oldEntity is null)
            {
                throw new ArgumentException(string.Format(_stringLocalizer["There is no {0} with {1} id."].Value, typeof(T).Name, id.ToString()));
            }
            UpdateHelper<TUpdateDto, T>.ReplaceUpdatedProperties(newEntity, oldEntity);
            return _mapper.Map<TDetailedDto>(await (await GetRepositoryAsync(cancellationToken)).UpdateAsync(oldEntity, cancellationToken));
        }

        public virtual Task<IEnumerable<TDetailedDto>> ReadAllEntitiesAsync(CancellationToken cancellationToken = default)
        {
            return ReadEntitiesByPredicateAsync(entity => true, PaginationParameters.All(), cancellationToken);
        }

        public virtual async Task<TDetailedDto?> ReadEntityByIdAsync(TIdDto id, string localizationCode, CancellationToken cancellationToken = default)
        {
            return _mapper.Map<TDetailedDto?>(
                await (await GetRepositoryAsync(cancellationToken))
                .ReadByIdAsync(GeneralIdExtractor<TIdDto, TIdAttribute>.GetId(id), localizationCode, cancellationToken));
        }

        public async Task<IEnumerable<TDetailedDto>> ReadEntitiesByPredicateAsync(Expression<Func<T, bool>> predicate, PaginationParameters paginationParameters, string localizationCode, CancellationToken cancellationToken = default)
        {
            var fetchedEntities = await (await GetRepositoryAsync(cancellationToken))
                .ReadByPredicateAsync(predicate, paginationParameters.GetTake(), paginationParameters.GetSkip(), localizationCode, cancellationToken);

            return _mapper.Map<IEnumerable<TDetailedDto>>(fetchedEntities);
        }

        public Task<IEnumerable<TDetailedDto>> ReadAllEntitiesAsync(string localizationCode, CancellationToken cancellationToken = default)
        {
            return ReadEntitiesByPredicateAsync(entity => true, PaginationParameters.All(), localizationCode, cancellationToken);
        }

        public async Task<IEnumerable<TDetailedDto>> ReadEntitiesByPredicateAsync(Expression<Func<T, bool>> predicate, PaginationParameters paginationParameters, IDictionary<string, string> orderBy, CancellationToken cancellationToken = default)
        {
            var mappedOrderBy = orderBy
                .Select(keyValue =>
                {
                    if (!keyValue.Key.Contains(OrderByConstants.Postfix))
                    {
                        return new KeyValuePair<Expression<Func<T, object>>, bool>();
                    }

                    var propertyExpression = GeneralIdExtractor<T, TIdAttribute>.GetPropertyExpression(keyValue.Key.Replace(OrderByConstants.Postfix, ""));
                    if (propertyExpression is null)
                    {
                        throw new ArgumentException(string.Format(_stringLocalizer["There is no {0} property"].Value, keyValue.Key));
                    }
                    return new KeyValuePair<Expression<Func<T, object>>, bool>(propertyExpression, keyValue.Value == OrderByConstants.Descending);
                })
                .Where(kv => kv.Key != null)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            var fetchedEntities = await (await GetRepositoryAsync(cancellationToken))
                .ReadByPredicateAsync(predicate, paginationParameters.GetTake(), paginationParameters.GetSkip(), mappedOrderBy, cancellationToken);

            return _mapper.Map<IEnumerable<TDetailedDto>>(fetchedEntities);
        }

        protected async Task<IRepository<T, Expression<Func<T, bool>>>> GetRepositoryAsync(CancellationToken cancellationToken = default)
        {
            _repository ??= await _unitOfWork.GetRepositoryAsync<T, Expression<Func<T, bool>>>(cancellationToken);
            return _repository;
        }

        protected async Task<TSpecificRepository> GetSpecificRepositoryAsync<TSpecificRepository>(CancellationToken cancellationToken = default)
        {
            if (!_specificRepositories.TryGetValue(typeof(TSpecificRepository), out object? repository) && repository is null)
            {
                repository = await _unitOfWork.GetSpecificRepository<TSpecificRepository>(cancellationToken);
            }
            return (TSpecificRepository)repository!;
        }

        public async Task<IEnumerable<TDetailedDto>> ReadEntitiesByPredicateAsync(Expression<Func<T, bool>> predicate, PaginationParameters paginationParameters, IDictionary<Expression<Func<T, object>>, bool> orderBy, CancellationToken cancellationToken = default)
        {
            return _mapper.Map<IEnumerable<TDetailedDto>>(await (await GetRepositoryAsync(cancellationToken))
                .ReadByPredicateAsync(predicate, paginationParameters.GetTake(), paginationParameters.GetSkip(), orderBy, cancellationToken));
        }
    }
}
