namespace BusinessLogicLayer.Abstractions.BaseInterfaces
{
    public interface ICreateService<TDetailedDto, TCreateDto>
    {
        Task<TDetailedDto> CreateEntityAsync(TCreateDto newEntity, CancellationToken cancellationToken = default);
    }
}
