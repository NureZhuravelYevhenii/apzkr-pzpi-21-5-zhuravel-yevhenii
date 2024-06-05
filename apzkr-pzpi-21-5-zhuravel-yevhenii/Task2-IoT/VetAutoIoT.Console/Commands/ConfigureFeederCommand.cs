using Spectre.Console;
using Spectre.Console.Cli;
using VetAutoIoT.Core.Configurations;

namespace VetAutoIoT.Console.Commands
{
    public class ConfigureFeederCommand : AsyncCommand
    {
        private readonly FeederConfiguration _feederConfiguration;

        public ConfigureFeederCommand(FeederConfiguration feederConfiguration)
        {
            _feederConfiguration = feederConfiguration;
        }

        public override Task<int> ExecuteAsync(CommandContext context)
        {
            var latitude = AnsiConsole.Ask<double>("Type latitude:");
            var longitude = AnsiConsole.Ask<double>("Type longitude:");

            _feederConfiguration.Latitude = latitude;
            _feederConfiguration.Longitude = longitude;

            return Task.FromResult(0);
        }
    }
}
