using BusinessLogicLayer.Abstractions.BaseInterfaces;
using BusinessLogicLayer.Entities.AnimalFeeders;
using BusinessLogicLayer.Entities.Feeders;
using BusinessLogicLayer.Entities.Pagination;
using DataAccessLayer.Entities;
using Core.Enums;

namespace BusinessLogicLayer.Abstractions
{
    public interface IAnimalFeederService : ICrudService<AnimalFeeder, AnimalFeederIdDto, AnimalFeederDto, AnimalFeederCreationDto, AnimalFeederUpdateDto>
    {
        Task<IEnumerable<FeederDto>> GetFeedersByPopularityAsync(CancellationToken cancellationToken = default);
        Task<double> GetAverageEatenFoodAmountAsync(Guid animalId, CancellationToken cancellationToken = default);
        Task<Month> GetMostPopularMonthForFeederAsync(Guid feederId, CancellationToken cancellationToken = default);
        Task<DayOfWeek> GetMostPopularDayOfWeekForFeederAsync(Guid feederId, CancellationToken cancellationToken = default);
        Task<Season> GetMostPopularSeasonForFeederAsync(Guid feederId, CancellationToken cancellationToken = default);
    }
}
