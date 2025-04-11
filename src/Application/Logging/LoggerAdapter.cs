namespace MAR.Application.Logging;

using Microsoft.Extensions.Logging;
using MAR.Domain.Interfaces;

public class LoggerAdapter<T> : ILoggerAdapter<T>
{
    private readonly ILogger<T> _logger;

    public LoggerAdapter(ILogger<T> logger)
    {
        _logger = logger;
    }

    void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, 
        Func<TState, Exception?, string> formatter)
    {
        string message = formatter(state, exception);
        
        switch (logLevel)
        {
            case LogLevel.Information:
                LogInformation((LogEvent)eventId.Id, message);
                break;
            case LogLevel.Warning:
                _logger.LogWarning(eventId.Id, message);
                break;
            case LogLevel.Error:
                _logger.LogError(eventId.Id, exception, message);
                break;
            default:
                _logger.Log(logLevel, eventId, state, exception, formatter);
                break;
        }
    }

    bool ILogger.IsEnabled(LogLevel logLevel)
    {
        return _logger.IsEnabled(logLevel);
    }

    IDisposable? ILogger.BeginScope<TState>(TState state)
    {
        return _logger.BeginScope(state);
    }


    public void LogInformation(LogEvent logEvent, string message)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation((int)logEvent, null, message);
        }
    }

    public void LogInformation<T0>(LogEvent logEvent, string message, T0 arg0)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation((int)logEvent, null, message, arg0);
        }
    }

    public void LogInformation<T0, T1>(LogEvent logEvent, string message, T0 arg0, T1 arg1)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation((int)logEvent, null, message, arg0, arg1);
        }
    }

    public void LogInformation<T0, T1, T2>(LogEvent logEvent, string message, T0 arg0, T1 arg1, T2 arg2)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation((int)logEvent, null, message, arg0, arg1, arg2);
        }
    }
}