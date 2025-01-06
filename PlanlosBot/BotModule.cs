using PlanlosBot.Logging;
using Serilog;
using Serilog.Events;
namespace PlanlosBot;

public class Modules
{
    
public class BotModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        LoggerConfiguration configuration = new LoggerConfiguration()
            .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
            .WriteTo.File("latest.log", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true);
        
        builder.RegisterInstance(configuration.CreateLogger())
            .As<ILogger>()
            .SingleInstance();
        builder.RegisterType<LogWrapper>()
            .As<ILogWrapper>();
}