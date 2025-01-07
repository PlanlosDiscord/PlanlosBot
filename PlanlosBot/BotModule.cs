using Autofac;
using Discord.Interactions;
using Discord.WebSocket;
using PlanlosBot.Contracts;
using PlanlosBot.Interactions.Modals;
using PlanlosBot.Interactions.TestInteractions;
using PlanlosBot.Logging;
using Serilog;
using Serilog.Events;

namespace PlanlosBot;

public class BotModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        LoggerConfiguration configuration = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("latest.log", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, restrictedToMinimumLevel: LogEventLevel.Information);
        
        builder.RegisterInstance(configuration.CreateLogger())
            .As<ILogger>()
            .SingleInstance();
        builder.RegisterType<LogWrapper>()
            .As<ILogWrapper>();
        
        DiscordSocketClient discordClient = new();
        builder.RegisterInstance(discordClient)
            .AsSelf()
            .AsImplementedInterfaces()
            .SingleInstance();

        InteractionServiceConfig interactionServiceConfig = new ();
        
        InteractionService interactionService = new(discordClient, interactionServiceConfig);
        RegisterTypeConverters(interactionService);
        builder.RegisterInstance(interactionService).AsSelf().SingleInstance();
        
        builder.RegisterType<BotHost>().AsSelf().SingleInstance();
        builder.RegisterType<NameModal>().AsSelf().SingleInstance();

        builder.RegisterType<TestCommands>().AsSelf();
    }

    private void RegisterTypeConverters(InteractionService interactionService)
    {
        //interactionService.AddTypeConverter<string[]>(new StringArrayConverter());
    }
}