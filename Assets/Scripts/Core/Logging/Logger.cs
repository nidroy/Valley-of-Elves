using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Центральный фасад логирования игры.
/// Отправляет сообщения в консоль и, при включении, в файл.
/// </summary>
public static class Logger
{
    private const string GameFolderName = "Valley of Elves";
    private const string LogsFolderName = "Logs";
    private const string LogFileName = "game.log";
    private const int MaxLogFileSizeMb = 5;

    private static readonly string LogFilePath =
        Path.Combine(
            Application.persistentDataPath,
            GameFolderName,
            LogsFolderName,
            LogFileName);

    private static ILogSink _consoleSink = new UnityConsoleLogSink();
    private static ILogSink _fileSink;
    private static bool _isFileLoggingEnabled = true;

    /// <summary>
    /// Признак включённого файлового логирования.
    /// </summary>
    public static bool IsFileLoggingEnabled => _isFileLoggingEnabled;

    /// <summary>
    /// Настраивает файловое логирование.
    /// </summary>
    public static void ConfigureFileLogging(bool enabled)
    {
        _isFileLoggingEnabled = enabled;
        _fileSink = enabled ? new FileLogSink(LogFilePath, MaxLogFileSizeMb) : null;
    }

    /// <summary>
    /// Записывает сообщение.
    /// </summary>
    public static void Log(LogLevel level, string source, string message)
    {
        try
        {
            _consoleSink?.Write(level, source, message);

            if (_isFileLoggingEnabled)
            {
                _fileSink?.Write(level, source, message);
            }
        }
        catch (Exception exception)
        {
            Debug.LogError($"Logger internal error: {exception.Message}");
        }
    }

    /// <summary>
    /// Очищает файл логов.
    /// </summary>
    public static void ClearLogFile()
    {
        try
        {
            if (File.Exists(LogFilePath))
            {
                File.Delete(LogFilePath);
            }
        }
        catch (Exception exception)
        {
            Debug.LogError($"Failed to clear log file: {exception.Message}");
        }
    }
}
