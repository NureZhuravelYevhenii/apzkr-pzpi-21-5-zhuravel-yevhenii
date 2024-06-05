using DataAccessLayer.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.EntityFramework
{
    public static class DependencyInjector
    {
        public static void Inject(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddDbContext<VetAutoContext>(options =>
            {
                options.UseSqlServer(configuration["ConnectionStrings:EfConnectionString"]);
            });
        }
    }
}
