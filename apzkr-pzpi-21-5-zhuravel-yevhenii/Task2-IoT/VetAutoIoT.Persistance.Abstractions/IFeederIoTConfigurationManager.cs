using VetAutoIoT.Core.Configurations;

namespace VetAutoIoT.Persistence.Abstractions
{
    public interface IFeederIoTConfigurationManager
    {
        Task<ApiConfiguration?> GetApiConfigurationAsync(CancellationToken cancellationToken = default);
        Task<FeederConfiguration?> GetFeederConfigurationAsync(CancellationToken cancellationToken = default);
        Task SetApiConfigurationAsync(ApiConfiguration apiConfiguration, CancellationToken cancellationToken = default);
        Task SetFeederConfigurationAsync(FeederConfiguration feederConfiguration, CancellationToken cancellationToken = default);
        Task ConfigureAsync(CancellationToken cancellationToken = default);
    }
}
