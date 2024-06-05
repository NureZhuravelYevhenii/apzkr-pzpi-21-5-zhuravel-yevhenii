using Core.Configurations;
using DataAccessLayer.Abstractions;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Reflection;

namespace DataAccessLayer.AdoNet
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;
        private IDictionary<Type, object> _cachedRepositories = new Dictionary<Type, object>();

        public UnitOfWork(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<IRepository<T, TPredicate>> GetRepositoryAsync<T, TPredicate>(CancellationToken cancellationToken = default)
            where T : class
        {
            var repositoryInterfaceType = typeof(IRepository<T, TPredicate>);

            if (_cachedRepositories.ContainsKey(repositoryInterfaceType))
            {

                return Task.FromResult((IRepository<T, TPredicate>)_cachedRepositories[repositoryInterfaceType]);
            }

            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException("Creating of {0} is cancelled.");
            }

            var repository = CreateRepository(repositoryInterfaceType);
            _cachedRepositories.Add(repositoryInterfaceType, repository);

            return Task.FromResult((IRepository<T, TPredicate>)repository);
        }

        public Task<T> GetSpecificRepository<T>(CancellationToken cancellationToken)
        {
            var repositoryInterfaceType = typeof(T);

            if (_cachedRepositories.ContainsKey(repositoryInterfaceType))
            {
                return Task.FromResult((T)_cachedRepositories[repositoryInterfaceType]);
            }

            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException("Creating of {0} is cancelled.");
            }

            var repository = CreateSpecificRepository(repositoryInterfaceType);
            _cachedRepositories.Add(repositoryInterfaceType, repository);

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

        private object CreateRepository(Type repositoryInterface)
        {
            var repositoryImplementation = GetRepositoryImplementationType(repositoryInterface);

            return CreateInstanceOfRepositoryImplementationType(repositoryImplementation);
        }

        private Type GetRepositoryImplementationType(Type repositoryInterface)
        {
            var repositoryImplementationType = typeof(UnitOfWork).Assembly
                .GetTypes()
                .FirstOrDefault(type =>
                    type.IsVisible &&
                    type.IsClass &&
                    !type.IsAbstract &&
                    type.IsAssignableTo(repositoryInterface));

            if (repositoryImplementationType is null)
            {
                throw new InvalidOperationException($"Unable to find repository implementation: {repositoryInterface.Name}.");
            }

            return repositoryImplementationType;
        }

        private object CreateInstanceOfRepositoryImplementationType(Type repositoryImplementationType)
        {
            var parameters = GetValidParametersList(repositoryImplementationType);

            var repository = Activator.CreateInstance(repositoryImplementationType, parameters);
            if (repository is null)
            {
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
                catch
                {
                    continue;
                }
            }
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
