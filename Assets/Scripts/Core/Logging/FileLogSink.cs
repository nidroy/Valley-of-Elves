using System;
using System.IO;

/// <summary>
/// Логирование сообщений в файл.
/// Этот класс отвечает только за файловый вывод.
/// </summary>
public sealed class FileLogSink : ILogSink
{
    private readonly string _filePath;
    private readonly object _lockObject = new object();
    private readonly long _maxFileSizeBytes;

    public FileLogSink(string filePath, int maxFileSizeMb)
    {
        _filePath = filePath;
        _maxFileSizeBytes = maxFileSizeMb * 1024L * 1024L;
        EnsureDirectoryExists();
    }

    public void Write(LogLevel level, string source, string message)
    {
        string formattedMessage = LoggerMessageFormatter.Format(level, source, message);

        lock (_lockObject)
        {
            RotateIfNeeded();
            File.AppendAllText(_filePath, formattedMessage + Environment.NewLine);
        }
    }

    private void EnsureDirectoryExists()
    {
        string directory = Path.GetDirectoryName(_filePath);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    private void RotateIfNeeded()
    {
        if (!File.Exists(_filePath))
        {
            return;
        }

        FileInfo fileInfo = new FileInfo(_filePath);

        if (fileInfo.Length >= _maxFileSizeBytes)
        {
            File.Delete(_filePath);
        }
    }
}