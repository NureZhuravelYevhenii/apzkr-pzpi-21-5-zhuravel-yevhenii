using Core.Enums;
using DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace DataAccessLayer.Abstractions.Repositories
{
    public interface IAnimalRepository : IRepository<Animal, Expression<Func<Animal, bool>>>
    {
        Task<int> GetNumberOfVisitedFeedersAsync(Guid animalId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Feeder>> GetVisitedFeederInSeasonAsync(Guid animalId, Season season = Season.Undefined, CancellationToken cancellationToken = default);
    }
}
