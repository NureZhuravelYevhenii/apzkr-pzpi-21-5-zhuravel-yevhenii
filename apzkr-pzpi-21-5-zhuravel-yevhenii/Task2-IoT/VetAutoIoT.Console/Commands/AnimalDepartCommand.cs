using Spectre.Console;
using Spectre.Console.Cli;
using VetAutoIoT.Abstractions;
using VetAutoIoT.Entities.EventArguments;

namespace VetAutoIoT.Console.Commands
{
    public class AnimalDepartCommand : AsyncCommand
    {
        private readonly IFeederService _feederService;

        public AnimalDepartCommand(IFeederService feederService)
        {
            _feederService = feederService;
        }

        public override async Task<int> ExecuteAsync(CommandContext context)
        {
            var animalId = AnsiConsole.Ask<Guid>("Provide animal id:");

            await _feederService.AnimalDepartAsync(new AnimalDepartArgs
            {
                Id = animalId,
            });

            return 0;
        }
    }
}
