namespace BusinessLogicLayer.Abstractions.BaseInterfaces
{
    public interface IDeleteService<TIdDto>
    {
        Task DeleteEntityAsync(TIdDto id, CancellationToken cancellationToken = default);
    }
}
