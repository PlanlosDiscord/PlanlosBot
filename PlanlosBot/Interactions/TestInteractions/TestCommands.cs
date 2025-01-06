using Discord.Interactions;
using Serilog;

namespace PlanlosBot.Interactions.TestInteractions;

public class TestCommands: InteractionModuleBase
{
    private readonly ILogger _logger;

    [SlashCommand("ping", "Pong!")]
    public async Task Command() => await RespondAsync("Pong!");

    public TestCommands(ILogger logger)
    {
        _logger = logger;
        _logger.Debug("TestCommands");
    }

    public override void OnModuleBuilding(InteractionService commandService, ModuleInfo module)
    {
        _logger.Debug("TestCommands::OnModuleBuilding");
    }
}