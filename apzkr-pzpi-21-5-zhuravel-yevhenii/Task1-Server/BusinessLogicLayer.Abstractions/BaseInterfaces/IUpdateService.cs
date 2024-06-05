namespace BusinessLogicLayer.Abstractions.BaseInterfaces
{
    public interface IUpdateService<TDetailedDto, TUpdateDto>
    {
        Task<TDetailedDto> UpdateEntityAsync(TUpdateDto newEntity, CancellationToken cancellationToken = default);
    }
}
