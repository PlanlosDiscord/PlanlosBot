using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using PlanlosBot.Attributes;
using PlanlosBot.Contracts;
using PlanlosBot.Interactions.Modals;
using PlanlosBot.Logging;

namespace PlanlosBot;

public class BotHost
{
    private readonly DiscordSocketClient _socketClient;
    private readonly InteractionService _interactionService;
    private readonly ILogWrapper _logWrapper;
    private readonly IServiceProvider _serviceProvider;

    public BotHost(DiscordSocketClient socketClient, InteractionService interactionService , Func<string, ILogWrapper> loggerFactory, IServiceProvider serviceProvider)
    {
        _socketClient = socketClient;
        _interactionService = interactionService;
        _logWrapper = loggerFactory(nameof(BotHost));
        _serviceProvider = serviceProvider;
    }

    public async Task Launch()
    {
        DiscordLogWrapper discordLogWrapper = new (_logWrapper); 
        _socketClient.Log += discordLogWrapper.Log;
        _interactionService.Log += discordLogWrapper.Log;

        await _interactionService.AddModulesAsync(typeof(BotHost).Assembly, _serviceProvider);
        
        await _socketClient.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DISCORD_TOKEN"));
        await _socketClient.StartAsync();

        await FindEventAttributes();
        
        _socketClient.InteractionCreated += async interaction =>
        {
            IInteractionContext context = new SocketInteractionContext(_socketClient, interaction);
            await _interactionService.ExecuteCommandAsync(context, _serviceProvider);
        };
        await Task.Delay(-1);
    }
    
    
    public async Task RegisterCommands()
    {
#if DEBUG
        await _interactionService.RegisterCommandsToGuildAsync(1032990109101981788);
#else
            await _interactionService.RegisterCommandsGloballyAsync();
#endif
        await _logWrapper.Log("Bot started");
    }

    private Task FindEventAttributes()
    {
        List<Task> tasks =
        [
            FindOnReady()
        ];
        return Task.WhenAll(tasks);
    }

    private Task FindOnReady()
    {
        MethodInfo[] methodInfos = typeof(BotHost).Assembly.GetTypes()
            .SelectMany(t => t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            .Where(m => m.GetCustomAttributes(typeof(OnReadyAttribute), true).Length > 0).ToArray();
        
        if (methodInfos.Length == 0) 
            return Task.CompletedTask;
        
        foreach (MethodInfo methodInfo in methodInfos)
        {
            object o = _serviceProvider.GetService(typeof(NameModal));
            _socketClient.Ready += (Func<Task>)methodInfo.CreateDelegate(typeof(Func<Task>), o);
        }
        return Task.CompletedTask;
    }
}