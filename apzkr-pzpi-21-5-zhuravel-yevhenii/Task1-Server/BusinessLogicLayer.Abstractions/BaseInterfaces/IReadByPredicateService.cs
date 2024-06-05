using BusinessLogicLayer.Entities.Pagination;
using System.Linq.Expressions;

namespace BusinessLogicLayer.Abstractions.BaseInterfaces
{
    public interface IReadByPredicateService<T, TDetailedDto>
    {
        Task<IEnumerable<TDetailedDto>> ReadEntitiesByPredicateAsync(Expression<Func<T, bool>> predicate, PaginationParameters paginationParameters, CancellationToken cancellationToken = default);
        Task<IEnumerable<TDetailedDto>> ReadEntitiesByPredicateAsync(Expression<Func<T, bool>> predicate, PaginationParameters paginationParameters, string localizationCode, CancellationToken cancellationToken = default);
        Task<IEnumerable<TDetailedDto>> ReadEntitiesByPredicateAsync(Expression<Func<T, bool>> predicate, PaginationParameters paginationParameters, IDictionary<string, string> orderBy, CancellationToken cancellationToken = default);
        Task<IEnumerable<TDetailedDto>> ReadEntitiesByPredicateAsync(Expression<Func<T, bool>> predicate, PaginationParameters paginationParameters, IDictionary<Expression<Func<T, object>>, bool> orderBy, CancellationToken cancellationToken = default);
    }
}
