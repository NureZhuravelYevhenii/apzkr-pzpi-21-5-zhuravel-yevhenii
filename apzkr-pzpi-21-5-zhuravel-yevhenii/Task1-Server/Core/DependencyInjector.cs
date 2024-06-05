using Core.Abstractions.Services.Security;
using Core.Services.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class DependencyInjector
    {
        public static void Inject(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddScoped<IPasswordHasher, PasswordService>();
        }
    }
}
