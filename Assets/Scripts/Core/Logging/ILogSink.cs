/// <summary>
/// Интерфейс получателя лог-сообщений.
/// </summary>
public interface ILogSink
{
    /// <summary>
    /// Обрабатывает одно сообщение.
    /// </summary>
    void Write(LogLevel level, string source, string message);
}