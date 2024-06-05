namespace BusinessLogicLayer.Abstractions.BaseInterfaces
{
    public interface IReadByIdService<TIdDto, TDetailedDto>
    {
        Task<TDetailedDto?> ReadEntityByIdAsync(TIdDto id, CancellationToken cancellationToken = default);
        Task<TDetailedDto?> ReadEntityByIdAsync(TIdDto id, string localizationCode, CancellationToken cancellationToken = default);
    }
}
