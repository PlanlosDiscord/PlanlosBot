using Discord.Interactions;
using Serilog;

namespace PlanlosBot.Interactions.TestInteractions;

public class CommandTest: InteractionModuleBase
{
    private readonly ILogger _logger;

    [SlashCommand("ping", "Pong!")]
    public async Task Command() => await RespondAsync("Pong!");

    public CommandTest(ILogger logger)
    {
        _logger = logger;
        _logger.Debug("CommandTest");
    }

    public override void OnModuleBuilding(InteractionService commandService, ModuleInfo module)
    {
        
        _logger.Debug("CommandTest::OnModuleBuilding");
    }
}