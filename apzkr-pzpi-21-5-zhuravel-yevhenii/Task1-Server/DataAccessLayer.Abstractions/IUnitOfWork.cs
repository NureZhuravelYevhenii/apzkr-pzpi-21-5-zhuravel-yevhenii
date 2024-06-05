namespace DataAccessLayer.Abstractions
{
    public interface IUnitOfWork
    {
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task<IRepository<T, TPredicate>> GetRepositoryAsync<T, TPredicate>(CancellationToken cancellationToken = default)
            where T : class;
        Task<T> GetSpecificRepository<T>(CancellationToken cancellationToken);
    }
}
