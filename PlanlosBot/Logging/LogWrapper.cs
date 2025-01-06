using PlanlosBot.Contracts;
using Serilog;
using Serilog.Events;

namespace PlanlosBot.Logging;

public class LogWrapper : ILogWrapper
{
    private readonly string _owner;
    private readonly ILogger _logger;

    public LogWrapper(string owner, ILogger logger)
    {
        _owner = owner;
        _logger = logger;
    }

    public Task Log(string message, string? sender = null, LogEventLevel severity = LogEventLevel.Information, Exception? exception = null)
    {
        string finalMessage = $"{sender ?? _owner}: {message}";
        _logger.Write(severity, exception, finalMessage);
        return Task.CompletedTask;
    }
}