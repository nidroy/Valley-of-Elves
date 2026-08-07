using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Класс для логирования сообщений приложения.
/// </summary>
public static class Logger
{
    /// <summary>
    /// Максимальный размер файла логов в мегабайтах.
    /// </summary>
    private static readonly int _maxLogFileSizeMB = Globals.MaxLogFileSizeMB;

    /// <summary>
    /// Полный путь к файлу логов.
    /// </summary>
    private static readonly string _logFilePath = Globals.LogFilePath;



    /// <summary>
    /// Лог-синк для вывода сообщений в консоль Unity.
    /// </summary>
    private static ILogSink _consoleLogSink = new UnityConsoleLogSink();

    /// <summary>
    /// Лог-синк для записи сообщений в файл.
    /// </summary>
    private static ILogSink _fileLogSink;



    /// <summary>
    /// Флаг, указывающий, включено ли файловое логирование.
    /// </summary>
    private static bool _isFileLoggingEnabled = true;

    /// <summary>
    /// Флаг возвращает текущее состояние файлового логирования.
    /// </summary>
    public static bool IsFileLoggingEnabled => _isFileLoggingEnabled;



    /// <summary>
    /// Метод включает или отключает запись логов в файл.
    /// </summary>
    /// <param name="enabled">true — включить файловое логирование, false — отключить.</param>
    public static void ToggleFileLogging(bool enabled)
    {
        _isFileLoggingEnabled = enabled;
        _fileLogSink = enabled ? new FileLogSink(_logFilePath, _maxLogFileSizeMB) : null;
    }



    /// <summary>
    /// Метод записывает сообщение лога в доступные каналы вывода.
    /// </summary>
    /// <param name="level">Уровень важности сообщения.</param>
    /// <param name="source">Источник сообщения.</param>
    /// <param name="message">Текст сообщения.</param>
    public static void Log(LogLevel level, string source, string message)
    {
        try
        {
            _consoleLogSink?.Write(level, source, message);

            if (_isFileLoggingEnabled)
            {
                _fileLogSink?.Write(level, source, message);
            }
        }
        catch (Exception exception)
        {
            Debug.LogError($"Logger internal error: {exception.Message}");
        }
    }
}