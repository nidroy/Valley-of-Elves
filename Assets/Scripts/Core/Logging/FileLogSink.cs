using System;
using System.IO;

/// <summary>
/// Реализация лог-синка для записи сообщений в файл.
/// </summary>
public class FileLogSink : ILogSink
{
    /// <summary>
    /// Полный путь к файлу логов.
    /// </summary>
    private readonly string _logFilePath;

    /// <summary>
    /// Максимальный размер файла в байтах.
    /// </summary>
    private readonly long _maxLogFileSizeBytes;

    /// <summary>
    /// Объект блокировки для синхронизации записи в файл.
    /// </summary>
    private readonly object _lockObject = new object();



    /// <summary>
    /// Конструктор создаёт новый файловый лог-синк.
    /// </summary>
    /// <param name="filePath">Путь к файлу логов.</param>
    /// <param name="maxFileSizeMb">Максимальный размер файла в мегабайтах.</param>
    public FileLogSink(string logFilePath, int maxLogFileSizeMB)
    {
        _logFilePath = logFilePath;
        _maxLogFileSizeBytes = maxLogFileSizeMB * 1024L * 1024L;

        CreateLogFileDirectory();
    }



    /// <summary>
    /// Метод записывает сообщение в файл.
    /// Перед записью проверяет размер файла и при необходимости очищает его.
    /// </summary>
    /// <param name="level">Уровень важности сообщения.</param>
    /// <param name="source">Источник сообщения.</param>
    /// <param name="message">Текст сообщения.</param>
    public void Write(LogLevel level, string source, string message)
    {
        string formattedMessage = LoggerMessageFormatter.Format(level, source, message);

        lock (_lockObject)
        {
            ValidateLogFileSize();

            File.AppendAllText(_logFilePath, formattedMessage + Environment.NewLine);
        }
    }



    /// <summary>
    /// Метод создаёт директорию для файла логов, если она ещё не существует.
    /// </summary>
    private void CreateLogFileDirectory()
    {
        string directory = Path.GetDirectoryName(_logFilePath);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    /// <summary>
    /// Метод проверяет размер файла логов.
    /// Если файл слишком большой, удаляет его, чтобы начать новый.
    /// </summary>
    private void ValidateLogFileSize()
    {
        if (!File.Exists(_logFilePath))
        {
            return;
        }

        FileInfo fileInfo = new FileInfo(_logFilePath);

        if (fileInfo.Length >= _maxLogFileSizeBytes)
        {
            File.Delete(_logFilePath);
        }
    }
}