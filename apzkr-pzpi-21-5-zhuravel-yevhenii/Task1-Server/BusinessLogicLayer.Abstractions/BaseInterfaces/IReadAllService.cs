namespace BusinessLogicLayer.Abstractions.BaseInterfaces
{
    public interface IReadAllService<TDetailed>
    {
        Task<IEnumerable<TDetailed>> ReadAllEntitiesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TDetailed>> ReadAllEntitiesAsync(string localizationCode, CancellationToken cancellationToken = default);
    }
}
