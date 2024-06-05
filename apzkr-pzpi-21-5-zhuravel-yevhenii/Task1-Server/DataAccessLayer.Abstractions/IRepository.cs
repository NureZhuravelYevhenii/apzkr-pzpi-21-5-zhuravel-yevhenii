using Core.EntityHelpers;
using System.Linq.Expressions;

namespace DataAccessLayer.Abstractions
{
    public interface IRepository<T, TPredicate>
    {
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
        Task<T> CreateAsync(T entity, string localizationCode, CancellationToken cancellationToken = default);
        Task<T?> ReadByIdAsync(EntityIds id, CancellationToken cancellationToken = default);
        Task<T?> ReadByIdAsync(EntityIds id, string localizationCode, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> ReadByPredicateAsync(TPredicate predicate, int take, int skip, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> ReadByPredicateAsync(TPredicate predicate, int take, int skip, string localizationCode, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> ReadByPredicateAsync(TPredicate predicate, int take, int skip, IDictionary<Expression<Func<T, object>>, bool> orderBy, CancellationToken cancellationToken = default);
        Task<int> CountEntitiesByPredicateAsync(TPredicate predicate, int take, int skip, CancellationToken cancellationToken = default);
        Task<int> CountEntitiesByPredicateAsync(TPredicate predicate, int take, int skip, string localizationCode, CancellationToken cancellationToken = default);
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task<T> UpdateAsync(T entity, string localizationCode, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(EntityIds id, CancellationToken cancellationToken = default);
        Task<bool> DeleteSeveralAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    }
}
