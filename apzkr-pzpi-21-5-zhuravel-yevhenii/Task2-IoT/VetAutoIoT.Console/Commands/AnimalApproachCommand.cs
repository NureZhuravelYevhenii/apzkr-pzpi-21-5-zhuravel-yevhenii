using Spectre.Console;
using Spectre.Console.Cli;
using VetAutoIoT.Abstractions;
using VetAutoIoT.Entities.EventArguments;

namespace VetAutoIoT.Console.Commands
{
    public class AnimalApproachCommand : AsyncCommand
    {
        private readonly IFeederService _feederService;

        public AnimalApproachCommand(IFeederService feederService)
        {
            _feederService = feederService;
        }

        public override async Task<int> ExecuteAsync(CommandContext context)
        {
            var id = AnsiConsole.Ask<Guid>("Provide animal id:");

            await _feederService.AnimalApproachingAsync(new AnimalApproachingArgs
            {
                AnimalId = id
            });

            return 0;
        }
    }
}
