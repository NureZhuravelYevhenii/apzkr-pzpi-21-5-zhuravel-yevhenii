using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VetAutoIoT.Core.Configurations;
using VetAutoIoT.Persistence.Abstractions;

namespace VetAutoIoT.Persistence
{
    public class FeederIotConfigurationManager : IFeederIoTConfigurationManager
    {
        private readonly PersistenceDbContext _dbContext;

        public FeederIotConfigurationManager(PersistenceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ConfigureAsync(CancellationToken cancellationToken = default)
        {
            if (!await _dbContext.Database.CanConnectAsync())
            {
                await _dbContext.Database.EnsureCreatedAsync();
            }
        }

        public Task<ApiConfiguration?> GetApiConfigurationAsync(CancellationToken cancellationToken = default)
            => _dbContext.ApiConfigurations.FirstOrDefaultAsync(cancellationToken);

        public Task<FeederConfiguration?> GetFeederConfigurationAsync(CancellationToken cancellationToken = default)
            => _dbContext.FeederConfigurations.FirstOrDefaultAsync(cancellationToken);

        public async Task SetApiConfigurationAsync(ApiConfiguration apiConfiguration, CancellationToken cancellationToken = default)
        {
            var apiConfigurationPersisted = await _dbContext.ApiConfigurations.FirstOrDefaultAsync(cancellationToken);

            if (apiConfigurationPersisted is not null)
            {
                _dbContext.Remove(apiConfigurationPersisted);
            }

            await _dbContext.AddAsync(apiConfiguration, cancellationToken);
        }

        public async Task SetFeederConfigurationAsync(FeederConfiguration feederConfiguration, CancellationToken cancellationToken = default)
        {
            var feederConfigurationPersisted = await _dbContext.FeederConfigurations.FirstOrDefaultAsync(cancellationToken);

            if (feederConfigurationPersisted is not null)
            {
                _dbContext.Remove(feederConfigurationPersisted);
            }

            await _dbContext.AddAsync(feederConfiguration, cancellationToken);
        }
    }
}
