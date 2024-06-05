using DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace DataAccessLayer.Abstractions.Repositories
{
    public interface IAnimalTypeRepository : IRepository<AnimalType, Expression<Func<AnimalType, bool>>>
    {
        Task<int> GetAverageVisitedFeederCountAsync(Guid animalTypeId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Feeder>> GetFeedersThatAnimalTypeVisitedInSeasonAsync(Guid animalTypeId, CancellationToken cancellationToken = default);
    }
}
