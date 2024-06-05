using Core.EntityHelpers;
using DataAccessLayer.Abstractions;
using DataAccessLayer.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

public class BaseRepository<T> : IRepository<T, Expression<Func<T, bool>>> where T : class
{
    protected readonly VetAutoContext _context;

    public BaseRepository(VetAutoContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _context.Set<T>().AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual async Task<T> CreateAsync(T entity, string localizationCode, CancellationToken cancellationToken = default)
    {
        return await CreateAsync(entity, cancellationToken);
    }

    public virtual async Task<T?> ReadByIdAsync(EntityIds id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(GeneralIdExtractor<T, KeyAttribute>.GetByIdPredicate(id), cancellationToken);
    }

    public virtual async Task<T?> ReadByIdAsync(EntityIds id, string localizationCode, CancellationToken cancellationToken = default)
    {
        return await ReadByIdAsync(id, cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> ReadByPredicateAsync(Expression<Func<T, bool>> predicate, int take, int skip, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<T>().Where(predicate).Skip(skip).Take(take);
        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> ReadByPredicateAsync(Expression<Func<T, bool>> predicate, int take, int skip, string localizationCode, CancellationToken cancellationToken = default)
    {
        return await ReadByPredicateAsync(predicate, take, skip, cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> ReadByPredicateAsync(Expression<Func<T, bool>> predicate, int take, int skip, IDictionary<Expression<Func<T, object>>, bool> orderBy, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<T>().Where(predicate);

        foreach (var order in orderBy)
        {
            if (order.Value)
                query = query.OrderBy(order.Key);
            else
                query = query.OrderByDescending(order.Key);
        }

        return await query.Skip(skip).Take(take).ToListAsync(cancellationToken);
    }

    public virtual async Task<int> CountEntitiesByPredicateAsync(Expression<Func<T, bool>> predicate, int take, int skip, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().Where(predicate).Skip(skip).Take(take).CountAsync(cancellationToken);
    }

    public virtual async Task<int> CountEntitiesByPredicateAsync(Expression<Func<T, bool>> predicate, int take, int skip, string localizationCode, CancellationToken cancellationToken = default)
    {
        return await CountEntitiesByPredicateAsync(predicate, take, skip, cancellationToken);
    }

    public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var oldEntity = await ReadByIdAsync(GeneralIdExtractor<T, KeyAttribute>.GetId(entity), cancellationToken);

        if (oldEntity is null)
        {
            return await CreateAsync(entity, cancellationToken);
        }

        var oldEntityEntry = _context.Entry(oldEntity);
        oldEntityEntry.CurrentValues.SetValues(entity);

        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity, string localizationCode, CancellationToken cancellationToken = default)
    {
        return await UpdateAsync(entity, cancellationToken);
    }

    public virtual async Task<bool> DeleteAsync(EntityIds id, CancellationToken cancellationToken = default)
    {
        var entity = await ReadByIdAsync(id, cancellationToken);
        if (entity == null)
            return false;

        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public virtual async Task<bool> DeleteSeveralAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var entitiesToDelete = _context.Set<T>().Where(predicate);
        _context.Set<T>().RemoveRange(entitiesToDelete);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

