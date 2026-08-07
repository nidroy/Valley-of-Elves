/// <summary>
/// Интерфейс для объектов, которые принимают и обрабатывают лог-сообщения.
/// </summary>
public interface ILogSink
{
    /// <summary>
    /// Метод записывает лог-сообщение указанного уровня от заданного источника.
    /// </summary>
    /// <param name="level">Уровень важности сообщения.</param>
    /// <param name="source">Источник сообщения, например имя класса или системы.</param>
    /// <param name="message">Текст лог-сообщения.</param>
    public void Write(LogLevel level, string source, string message);
}