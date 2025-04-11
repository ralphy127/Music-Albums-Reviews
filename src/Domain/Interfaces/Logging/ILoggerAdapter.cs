namespace MAR.Domain.Interfaces;

using Microsoft.Extensions.Logging;

public interface ILoggerAdapter<T> : ILogger<T>
{
    void LogInformation(LogEvent logEvent, string message);

    void LogInformation<T0>(LogEvent logEvent, string message, T0 arg0);

    void LogInformation<T0, T1>(LogEvent logEvent, string message, T0 arg0, T1 arg1);

    void LogInformation<T0, T1, T2>(LogEvent logEvent, string message, T0 arg0, T1 arg1, T2 arg2);
}