using Discord;
using PlanlosBot.Contracts;
using Serilog.Events;

namespace PlanlosBot.Logging;

public class DiscordLogWrapper
{
    private readonly ILogWrapper _logger;

    public DiscordLogWrapper(ILogWrapper logger)
    {
        _logger = logger;
    }

    public Task Log(LogMessage msg)
    {
        _logger.Log(msg.Message, msg.Source, ConvertLogLevel(msg.Severity), msg.Exception);
        return Task.CompletedTask;
    }

    private static LogEventLevel ConvertLogLevel(LogSeverity severity)
    {
        return (LogEventLevel) 5 - (int) severity;
    }
}