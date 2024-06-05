using Spectre.Console.Cli;
using VetAutoIoT.Abstractions;
using VetAutoIoT.Core.Configurations;

namespace VetAutoIoT.Console.Commands
{
    public class ConfigureCommand : AsyncCommand
    {
        private readonly IConfigurationService _configurationService;
        private readonly ApiConfiguration _apiConfiguration;

        public ConfigureCommand(IConfigurationService configurationService, ApiConfiguration apiConfiguration)
        {
            _configurationService = configurationService;
            _apiConfiguration = apiConfiguration;
        }

        public override async Task<int> ExecuteAsync(CommandContext context)
        {
            var apiConfiguration = await _configurationService.GetConfigurationAsync();

            if (apiConfiguration is not null)
            {
                _apiConfiguration.SetPrototypeValues(apiConfiguration);
            }

            return 0;
        }
    }
}
