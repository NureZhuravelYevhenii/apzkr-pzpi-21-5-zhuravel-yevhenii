using DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace DataAccessLayer.Abstractions.Repositories
{
    public interface IAnimalFeederRepository : IRepository<AnimalFeeder, Expression<Func<AnimalFeeder, bool>>>
    {
        Task<IEnumerable<Feeder>> GetFeedersByPopularityAsync(int take, CancellationToken cancellationToken = default);
    }
}
