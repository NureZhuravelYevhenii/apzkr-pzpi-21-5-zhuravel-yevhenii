using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;

namespace VetAutoIoT.Console.Commands
{
    public class DefaultCommand : AsyncCommand
    {
        private const string TerminationCommand = "stop";

        private readonly IServiceProvider _serviceProvider;

        private readonly IDictionary<string, Type> _commands = new Dictionary<string, Type>
        {
            {"approach", typeof(AnimalApproachCommand) },
            {"depart", typeof(AnimalDepartCommand) },
            {"eat", typeof(AnimalEatCommand) },
            {"configure", typeof(ConfigureCommand) },
            {"configure-feeder", typeof(ConfigureCommand) },
        };

        public DefaultCommand(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override async Task<int> ExecuteAsync(CommandContext context)
        {
            while (true)
            {
                var commandText = AnsiConsole.Ask<string>("Type command");

                var commandArgs = commandText.Split(' ');

                if (commandArgs[0] == TerminationCommand)
                {
                    break;
                }

                if (!_commands.ContainsKey(commandArgs[0]))
                {
                    AnsiConsole.WriteLine($"There is no such commands. Try one of this: {string.Join(", ", _commands.Keys)}");
                    continue;
                }

                var command = _serviceProvider.GetRequiredService(_commands[commandArgs[0]]) as AsyncCommand;

                if (command is null)
                {
                    continue;
                }

                try
                {
                    await command.ExecuteAsync(context);
                }
                catch (Exception ex)
                {
                    AnsiConsole.WriteLine(ex.Message);
                }
            }

            var endCommand = _serviceProvider.GetRequiredService<AllAnimalDepartCommand>();

            await endCommand.ExecuteAsync(context);

            return 0;
        }
    }
}
