using Spectre.Console;
using Spectre.Console.Cli;
using VetAutoIoT.Abstractions;
using VetAutoIoT.Entities.EventArguments;

namespace VetAutoIoT.Console.Commands
{
    public class AnimalEatCommand : AsyncCommand
    {
        private readonly IFeederService _feederService;

        public AnimalEatCommand(IFeederService feederService)
        {
            _feederService = feederService;
        }

        public override async Task<int> ExecuteAsync(CommandContext context)
        {
            var id = AnsiConsole.Ask<Guid>("Provide animal id:");
            var amountOfFood = AnsiConsole.Ask<double>("Provide amount of eaten food:");

            await _feederService.AnimalEatsAsync(new AnimalEatsArgs
            {
                Id = id,
                AmountOfFood = amountOfFood,
            });

            return 0;
        }
    }
}
