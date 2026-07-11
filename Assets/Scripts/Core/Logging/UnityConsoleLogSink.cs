using UnityEngine;

/// <summary>
/// Логирование сообщений в консоль Unity.
/// </summary>
public sealed class UnityConsoleLogSink : ILogSink
{
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