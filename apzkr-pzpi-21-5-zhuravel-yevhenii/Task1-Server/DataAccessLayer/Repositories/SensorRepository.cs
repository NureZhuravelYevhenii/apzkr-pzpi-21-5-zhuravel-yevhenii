using Core.Configurations;
using Core.Localizations;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.BaseRepositories;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace DataAccessLayer.Repositories
{
    public class SensorRepository : BaseRepository<Sensor>
    {
        public SensorRepository(IOptions<MongoDbConfiguration> mongoDbConfigurationOptions, IStringLocalizer<Resource> stringLocalizer) : base(mongoDbConfigurationOptions, stringLocalizer)
        {
        }
    }
}
