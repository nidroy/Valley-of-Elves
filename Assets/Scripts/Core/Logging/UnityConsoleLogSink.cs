using UnityEngine;

/// <summary>
/// Реализация лог-синка для вывода сообщений в консоль Unity.
/// Перенаправляет сообщения в Debug.Log, Debug.LogWarning и Debug.LogError в зависимости от уровня логирования.
/// </summary>
public class UnityConsoleLogSink : ILogSink
{
    /// <summary>
    /// Метод записывает сообщение в консоль Unity.
    /// </summary>
    /// <param name="level">Уровень важности сообщения.</param>
    /// <param name="source">Источник сообщения.</param>
    /// <param name="message">Текст сообщения.</param>
    public void Write(LogLevel level, string source, string message)
    {
        string formattedMessage = LoggerMessageFormatter.Format(level, source, message);

        switch (level)
        {
            case LogLevel.Info:
                Debug.Log(formattedMessage);
                break;

            case LogLevel.Warning:
                Debug.LogWarning(formattedMessage);
                break;

            case LogLevel.Error:
                Debug.LogError(formattedMessage);
                break;
        }
    }
}