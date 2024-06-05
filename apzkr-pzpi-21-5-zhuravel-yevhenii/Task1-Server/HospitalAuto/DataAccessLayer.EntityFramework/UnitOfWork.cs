using Core.Localizations;
using DataAccessLayer.Abstractions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace DataAccessLayer.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<UnitOfWork> _logger;
        private readonly IStringLocalizer<Resource> _stringLocalizer;
        private IDictionary<Type, object> _cachedRepositories = new Dictionary<Type, object>();

        public UnitOfWork(
            IServiceProvider serviceProvider,
            ILogger<UnitOfWork> logger,
            IStringLocalizer<Resource> stringLocalizer)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _stringLocalizer = stringLocalizer;
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Commiting...");

            return Task.CompletedTask;
        }

        public Task<IRepository<T, TPredicate>> GetRepositoryAsync<T, TPredicate>(CancellationToken cancellationToken = default)
            where T : class
        {
            var repositoryInterfaceType = typeof(IRepository<T, TPredicate>);

            _logger.LogInformation($"Getting {repositoryInterfaceType.Name} repository.");

            if (_cachedRepositories.ContainsKey(repositoryInterfaceType))
            {
                _logger.LogInformation($"{repositoryInterfaceType.Name} repository received from cache.");

                return Task.FromResult((IRepository<T, TPredicate>)_cachedRepositories[repositoryInterfaceType]);
            }

            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError($"Creating of {repositoryInterfaceType.Name} is cancelled.");
                throw new OperationCanceledException(string.Format(_stringLocalizer["Creating of {0} is cancelled."].Value, repositoryInterfaceType.Name));
            }

            var repository = CreateRepository<T>(repositoryInterfaceType);
            _cachedRepositories.Add(repositoryInterfaceType, repository);

            _logger.LogInformation($"{repositoryInterfaceType.Name} repository created.");

            return Task.FromResult((IRepository<T, TPredicate>)repository);
        }

        public Task<T> GetSpecificRepository<T>(CancellationToken cancellationToken)
        {
            var repositoryInterfaceType = typeof(T);

            _logger.LogInformation($"Getting {repositoryInterfaceType.Name} repository.");

            if (_cachedRepositories.ContainsKey(repositoryInterfaceType))
            {
                _logger.LogInformation($"{repositoryInterfaceType.Name} repository received from cache.");

                return Task.FromResult((T)_cachedRepositories[repositoryInterfaceType]);
            }

            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError($"Creating of {repositoryInterfaceType.Name} is cancelled.");
                throw new OperationCanceledException(string.Format(_stringLocalizer["Creating of {0} is cancelled."].Value, repositoryInterfaceType.Name));
            }

            var repository = CreateSpecificRepository(repositoryInterfaceType);
            _cachedRepositories.Add(repositoryInterfaceType, repository);

            _logger.LogInformation($"{repositoryInterfaceType.Name} repository created.");

            return Task.FromResult((T)repository);
        }

        private object CreateSpecificRepository(Type repositoryInterface)
        {
            var repositoryImplementation = GetSpecificRepositoryImplementationType(repositoryInterface);
            return CreateInstanceOfRepositoryImplementationType(repositoryImplementation);
        }

        private Type GetSpecificRepositoryImplementationType(Type repositoryInterface)
        {
            var repositoryImplementationType = typeof(UnitOfWork).Assembly
                .GetTypes()
                .Where(t => t.IsAssignableTo(repositoryInterface))
                .FirstOrDefault()
                ??
                throw new InvalidOperationException($"Unable to find implementation type for {repositoryInterface.Name} interface.");
            return repositoryImplementationType;
        }

        private object CreateRepository<T>(Type repositoryInterface)
            where T : class
        {
            _logger.LogInformation($"Creating {repositoryInterface.Name} repository instance.");

            var repositoryImplementation = GetRepositoryImplementationType<T>(repositoryInterface);

            return CreateInstanceOfRepositoryImplementationType(repositoryImplementation);
        }

        private Type GetRepositoryImplementationType<T>(Type repositoryInterface)
            where T : class
        {
            _logger.LogInformation($"Getting {repositoryInterface.Name} implementation type.");

            var repositoryImplementationType = typeof(UnitOfWork).Assembly
                .GetTypes()
                .FirstOrDefault(type =>
                    type.IsVisible &&
                    type.IsClass && 
                    !type.IsAbstract && 
                    type.IsAssignableTo(repositoryInterface))
                ?? typeof(BaseRepository<T>);

            if (repositoryImplementationType is null)
            {
                _logger.LogError($"Unable to find repository implementation: {repositoryInterface.Name}.");

                throw new InvalidOperationException($"Unable to find repository implementation: {repositoryInterface.Name}.");
            }

            return repositoryImplementationType;
        }

        private object CreateInstanceOfRepositoryImplementationType(Type repositoryImplementationType)
        {
            _logger.LogInformation($"Creating {repositoryImplementationType.Name} instance.");

            var parameters = GetValidParametersList(repositoryImplementationType);

            var repository = Activator.CreateInstance(repositoryImplementationType, parameters);
            if (repository is null)
            {
                _logger.LogError($"Unable to create instance of repository implementation: {repositoryImplementationType.Name}.");

                throw new InvalidOperationException($"Unable to create instance of repository implementation: {repositoryImplementationType.Name}.");
            }

            return repository;
        }

        private object[] GetValidParametersList(Type type)
        {
            var constructors = type.GetConstructors();

            foreach (var constructorInfo in constructors)
            {
                try
                {
                    return GenerateConstructorParameters(constructorInfo);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    continue;
                }
            }

            _logger.LogError($"Unable create parameters for {type.Name} constructor.");
            throw new InvalidOperationException($"Unable to create parameters for {type.Name} constructor.");
        }

        private object[] GenerateConstructorParameters(ConstructorInfo constructorInfo)
        {
            var parameters = constructorInfo.GetParameters();
            var result = new List<object>();

            foreach (var parameter in parameters)
            {
                var parameterInstance = _serviceProvider.GetService(parameter.ParameterType)
                    ?? parameter.DefaultValue;
                if (parameterInstance is null)
                {
                    throw new InvalidOperationException($"Unable to instantiate {parameter.ParameterType.Name}.");
                }
                result.Add(parameterInstance);
            }

            return result.ToArray();
        }
    }
}
