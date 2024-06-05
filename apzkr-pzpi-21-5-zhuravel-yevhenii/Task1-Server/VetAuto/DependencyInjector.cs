using Core.Configurations;

namespace VetAuto
{
    public static class DependencyInjector
    {
        public static void Inject(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<JwtConfiguration>(configuration.GetSection("JwtConfiguration"));
            serviceCollection.Configure<MongoDbConfiguration>(configuration.GetSection("MongoDbConfiguration"));
            serviceCollection.Configure<AdoNetConfiguration>(configuration.GetSection("AdoNetConfiguration"));
        }
    }
}
