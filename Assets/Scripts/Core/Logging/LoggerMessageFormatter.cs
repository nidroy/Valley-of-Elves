using System;

/// <summary>
/// Формирует единый текст сообщения для всех лог-источников.
/// </summary>
public static class LoggerMessageFormatter
{
    /// <summary>
    /// Создаёт строку лога с датой, уровнем и источником.
    /// </summary>
    public static string Format(LogLevel level, string source, string message)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            source = "Unknown";
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            message = "Empty message";
        }

        return $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] [{source}] {message}";
    }
}