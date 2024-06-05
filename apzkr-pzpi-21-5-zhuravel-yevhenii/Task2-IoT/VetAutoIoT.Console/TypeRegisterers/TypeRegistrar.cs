using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace VetAutoIoT.Console.TypeRegisterers
{
    public class TypeRegistrar : ITypeRegistrar
    {
        private readonly IServiceCollection _serviceCollection;

        public TypeRegistrar(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public ITypeResolver Build()
        {
            return new TypeResolver(_serviceCollection.BuildServiceProvider());
        }

        public void Register(Type service, Type implementation)
        {
            _serviceCollection.AddScoped(service, implementation);
        }

        public void RegisterInstance(Type service, object implementation)
        {
            _serviceCollection.AddSingleton(service, implementation);
        }

        public void RegisterLazy(Type service, Func<object> factory)
        {
            _serviceCollection.AddScoped(service, sp => factory());
        }
    }
}
