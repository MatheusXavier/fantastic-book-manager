namespace Identity.API.Infrastructure.Logging;

public static class LoggerExtensions
{
    private static readonly Action<ILogger, string, string, Exception?> _informationMessages;
    private static readonly Action<ILogger, string, string, Exception> _exceptions;

    static LoggerExtensions()
    {
        _informationMessages = LoggerMessage.Define<string, string>(
            logLevel: LogLevel.Information,
            eventId: new EventId(1, "Identity.API"),
            formatString: "[{Context}] - {Message}");

        _exceptions = LoggerMessage.Define<string, string>(
            logLevel: LogLevel.Error,
            eventId: new EventId(2, "Identity.API"),
            formatString: "[{Context}] - It occurred an error with message: {Message}");
    }

    public static void LogInformationMessage<T>(this ILogger logger, string message)
        where T : class
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            _informationMessages(logger, typeof(T).Name, message, null);
        }
    }

    public static void LogException<T>(this ILogger logger, Exception exception)
        where T : class
    {
        if (logger.IsEnabled(LogLevel.Error))
        {
            _exceptions(logger, typeof(T).Name, exception.Message, exception);
        }
    }
}
