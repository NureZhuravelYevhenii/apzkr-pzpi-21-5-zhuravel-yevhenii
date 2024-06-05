using Core.EntityHelpers;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccessLayer.EntityFramework.Repositories
{
    public class AnimalCenterRepository : BaseRepository<AnimalCenter>
    {
        public AnimalCenterRepository(VetAutoContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<AnimalCenter>> ReadByPredicateAsync(Expression<Func<AnimalCenter, bool>> predicate, int take, int skip, CancellationToken cancellationToken = default)
        {
            var query = _context.AnimalCenters.Include(ac => ac.Animals).Where(predicate).Skip(skip).Take(take);
            return await query.ToListAsync(cancellationToken);
        }

        public override async Task<IEnumerable<AnimalCenter>> ReadByPredicateAsync(Expression<Func<AnimalCenter, bool>> predicate, int take, int skip, IDictionary<Expression<Func<AnimalCenter, object>>, bool> orderBy, CancellationToken cancellationToken = default)
        {
            var query = _context.AnimalCenters.Include(ac => ac.Animals).Where(predicate);

            foreach (var order in orderBy)
            {
                if (order.Value)
                    query = query.OrderBy(order.Key);
                else
                    query = query.OrderByDescending(order.Key);
            }

            return await query.Skip(skip).Take(take).ToListAsync(cancellationToken);
        }

        public override Task<bool> DeleteAsync(EntityIds id, CancellationToken cancellationToken = default)
        {
            var animals = _context.Animals.Where(a => a.AnimalCenterId == (Guid)id["Id"]);

            _context.Animals.RemoveRange(animals);

            return base.DeleteAsync(id, cancellationToken);
        }
    }
}
