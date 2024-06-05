using Spectre.Console.Cli;
using VetAutoIoT.Abstractions;
using VetAutoIoT.Core.Configurations;
using VetAutoIoT.Persistence.Abstractions;

namespace VetAutoIoT.Console.Commands
{
    public class AllAnimalDepartCommand : AsyncCommand
    {
        private readonly IFeederService _feederService;
        private readonly IFeederIoTConfigurationManager _configurationManager;
        private readonly FeederConfiguration _feederConfiguration;
        private readonly ApiConfiguration _apiConfiguration;

        public AllAnimalDepartCommand(
            IFeederService feederService,
            IFeederIoTConfigurationManager configurationManager,
            FeederConfiguration feederConfiguration,
            ApiConfiguration apiConfiguration
            )
        {
            _feederService = feederService;
            _configurationManager = configurationManager;
            _feederConfiguration = feederConfiguration;
            _apiConfiguration = apiConfiguration;
        }

        public override async Task<int> ExecuteAsync(CommandContext context)
        {
            await _feederService.AllAnimalDepartAsync();

            await _configurationManager.SetApiConfigurationAsync(_apiConfiguration);
            await _configurationManager.SetFeederConfigurationAsync(_feederConfiguration);

            return 0;
        }
    }
}
