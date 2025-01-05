using Discord;
using Serilog;

namespace PlanlosBot;

public class DiscordLogWrapper
{
    private readonly ILogger _logger;

    public DiscordLogWrapper(ILogger logger)
    {
        _logger = logger;
    }

    public Task Log(LogMessage msg)
    {
        switch (msg.Severity)
        {
            case LogSeverity.Critical:
                _logger.Fatal(msg.Message);
                break;
            case LogSeverity.Error:
                _logger.Error(msg.Message);
                break;
            case LogSeverity.Warning:
                _logger.Warning(msg.Message);
                break;
            case LogSeverity.Info:
                _logger.Information(msg.Message);
                break;
            case LogSeverity.Verbose:
                _logger.Verbose(msg.Message);
                break;
            case LogSeverity.Debug:
                _logger.Debug(msg.Message);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return Task.CompletedTask;
    }
}