using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using PlanlosBot.Contracts;
using PlanlosBot.Logging;

namespace PlanlosBot;

public class BotHost
{
    private readonly DiscordSocketClient _socketClient;
    private readonly InteractionService _interactionService;
    private readonly ILogWrapper _logWrapper;
    private readonly IServiceProvider _serviceProvider;
    private readonly DiscordLogWrapper _discordLogWrapper;

    public BotHost(DiscordSocketClient socketClient, InteractionService interactionService , Func<string, ILogWrapper> loggerFactory, IServiceProvider serviceProvider)
    {
        _socketClient = socketClient;
        _interactionService = interactionService;
        _logWrapper = loggerFactory(nameof(BotHost));
        _serviceProvider = serviceProvider;
        _discordLogWrapper = new DiscordLogWrapper(_logWrapper);
    }

    public async Task Launch()
    {
        _socketClient.Log += _discordLogWrapper.Log;
        _interactionService.Log += _discordLogWrapper.Log;

        await _interactionService.AddModulesAsync(typeof(BotHost).Assembly, _serviceProvider);
        
        await _socketClient.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DISCORD_TOKEN"));
        await _socketClient.StartAsync();

        _socketClient.Ready += async () =>
        {
#if DEBUG
            await _interactionService.RegisterCommandsToGuildAsync(1032990109101981788);
#else
            await _interactionService.RegisterCommandsGloballyAsync();
#endif
            await _logWrapper.Log("Bot started");
        };
        
        _socketClient.InteractionCreated += async interaction =>
        {
            IInteractionContext context = new SocketInteractionContext(_socketClient, interaction);
            await _interactionService.ExecuteCommandAsync(context, _serviceProvider);
        };
        
        await Task.Delay(-1);
    }
    
    

}