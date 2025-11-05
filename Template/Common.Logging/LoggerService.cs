using $safeprojectname$.Common.Logging;
using $safeprojectname$.Common.Extensions;
using $safeprojectname$.Common.Results;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace $safeprojectname$.Common.Logging
{
    public class LoggerService<T> : ILoggerService<T>
    {
        private readonly ILogger<T> _logger;

        public LoggerService(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void Log(string message, SeverityLevel level, Exception? exception = null)
        {
            // 1. Persistência com ILogger<T>
            var logLevel = MapSeverityToLogLevel(level);
            _logger.Log(logLevel, exception, "[{Context}] {Message}", typeof(T).Name, message);

            // 2. Exibição no console com cor
            WriteToConsole(level, message, exception);
        }

        public void Log(Result result, string? area = null, SeverityLevel level = SeverityLevel.Error)
        {
            if (result.Succeeded) return;

            var message = result.GetMessageOrError();
            var json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = false });

            Log($"[{area ?? typeof(T).Name}] {message} | Details: {json}", level, result.Exception);
        }

        public void Log<TData>(Result<TData> result, string? area = null, SeverityLevel level = SeverityLevel.Error)
        {
            if (result.Succeeded) return;

            var message = result.GetMessageOrError();
            var json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = false });

            Log($"[{area ?? typeof(T).Name}] {message} | Details: {json}", level, result.Exception);
        }

        private static LogLevel MapSeverityToLogLevel(SeverityLevel level) =>
            level switch
            {
                SeverityLevel.Verbose => LogLevel.Trace,
                SeverityLevel.Information => LogLevel.Information,
                SeverityLevel.Warning => LogLevel.Warning,
                SeverityLevel.Error => LogLevel.Error,
                SeverityLevel.Critical => LogLevel.Critical,
                _ => LogLevel.Information
            };

        private void WriteToConsole(SeverityLevel level, string message, Exception? exception = null)
        {
            var originalColor = Console.ForegroundColor;

            Console.ForegroundColor = level switch
            {
                SeverityLevel.Information => ConsoleColor.Cyan,
                SeverityLevel.Warning => ConsoleColor.Yellow,
                SeverityLevel.Error => ConsoleColor.Red,
                SeverityLevel.Critical => ConsoleColor.DarkRed,
                SeverityLevel.Verbose => ConsoleColor.Gray,
                _ => ConsoleColor.White
            };

            var prefix = $"[{level.ToString().ToUpper()}]";
            Console.WriteLine($"{prefix} {message}");

            if (exception is not null)
                Console.WriteLine($"Exception: {exception.Message}");

            Console.ForegroundColor = originalColor;
        }
    }
}
