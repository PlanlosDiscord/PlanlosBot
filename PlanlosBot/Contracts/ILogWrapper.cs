using Serilog.Events;

namespace PlanlosBot.Contracts;

public interface ILogWrapper
{
    Task Log(string message, string? sender = null, LogEventLevel severity = LogEventLevel.Information, Exception? exception = null);
}