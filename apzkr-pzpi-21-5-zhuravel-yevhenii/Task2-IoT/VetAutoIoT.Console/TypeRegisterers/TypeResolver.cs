using Spectre.Console.Cli;

namespace VetAutoIoT.Console.TypeRegisterers
{
    public class TypeResolver : ITypeResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public TypeResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object? Resolve(Type? type)
        {
            if (type is null)
            {
                return null;
            }

            return _serviceProvider.GetService(type);
        }
    }
}
