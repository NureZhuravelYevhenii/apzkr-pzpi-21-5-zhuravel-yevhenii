using VetAutoIoT.Core.Configurations;

namespace VetAutoIoT.Abstractions
{
    public interface IConfigurationService
    {
        Task<ApiConfiguration?> GetConfigurationAsync(CancellationToken cancellationToken = default);
    }
}
