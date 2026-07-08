using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Класс отвечает за запись сообщений игры в консоль и файл логов.
/// </summary>
public static class Logger
{
    #region Константы

    /// <summary>
    /// Класс хранит константы логирования.
    /// </summary>
    private static class Constants
    {
        // Название папки игры для хранения пользовательских данных.
        public const string GameFolderName = "Valley of Elves";

        // Название файла логов.
        public const string LogFileName = "log.txt";

        // Максимальный размер файла логов в байтах (5 MB).
        public const long MaxLogFileSize = 5 * 1024 * 1024;
    }

    #endregion


    #region Приватные поля

    // Полный путь к файлу логов.
    private static readonly string _logFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        Constants.GameFolderName,
        Constants.LogFileName
    );

    #endregion


    #region Публичные свойства

    /// <summary>
    /// Свойство получает или устанавливает состояние записи логов в файл.
    /// </summary>
    public static bool IsFileLoggingEnabled { get; set; } = true;

    #endregion


    #region Внутренние классы

    /// <summary>
    /// Класс содержит доступные уровни логирования.
    /// </summary>
    public enum LogLevel
    {
        // Информационное сообщение.
        Info,

        // Предупреждение.
        Warning,

        // Ошибка.
        Error
    }

    #endregion


    #region Публичные методы

    /// <summary>
    /// Метод записывает сообщение указанного уровня.
    /// </summary>
    /// <param name="logLevel">Уровень важности сообщения.</param>
    /// <param name="module">Название модуля, отправившего сообщение.</param>
    /// <param name="message">Текст сообщения.</param>
    public static void Log(LogLevel logLevel, string module, string message)
    {
        // Формируем текст сообщения лога.
        string logMessage = FormatLogMessage(logLevel, module, message);

#if UNITY_EDITOR
        // Выводим сообщение в консоль Unity.
        LogToUnityConsole(logLevel, logMessage);
#endif

        // Проверяем разрешение записи логов в файл.
        if (IsFileLoggingEnabled)
        {
            _ = SaveLogToFileAsync(logMessage);
        }
    }


    /// <summary>
    /// Метод удаляет файл логов.
    /// </summary>
    public static void DeleteLogFile()
    {
        try
        {
            // Проверяем существование файла логов.
            if (!File.Exists(_logFilePath))
            {
#if UNITY_EDITOR
                Debug.LogWarning("Log file does not exist.");
#endif
                return;
            }


            // Удаляем файл логов.
            File.Delete(_logFilePath);

#if UNITY_EDITOR
            Debug.Log("Log file deleted successfully.");
#endif
        }
        catch (Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError($"Failed to delete log file: {exception.Message}");
#endif
        }
    }

    #endregion


    #region Приватные методы записи

    /// <summary>
    /// Метод формирует строку сообщения лога.
    /// </summary>
    /// <param name="logLevel">Уровень важности сообщения.</param>
    /// <param name="module">Название модуля, отправившего сообщение.</param>
    /// <param name="message">Текст сообщения.</param>
    /// <returns>Сформированная строка лога.</returns>
    private static string FormatLogMessage(LogLevel logLevel, string module, string message)
    {
        // Получаем текущее время записи.
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // Формируем итоговое сообщение.
        return $"{timestamp} [{logLevel}] {module}: {message}";
    }


    /// <summary>
    /// Метод выводит сообщение в консоль Unity.
    /// </summary>
    /// <param name="logLevel">Уровень важности сообщения.</param>
    /// <param name="message">Текст сообщения.</param>
    private static void LogToUnityConsole(LogLevel logLevel, string message)
    {
        switch (logLevel)
        {
            case LogLevel.Info:
                Debug.Log(message);
                break;

            case LogLevel.Warning:
                Debug.LogWarning(message);
                break;

            case LogLevel.Error:
                Debug.LogError(message);
                break;
        }
    }


    /// <summary>
    /// Метод асинхронно сохраняет сообщение в файл логов.
    /// </summary>
    /// <param name="logMessage">Сообщение для сохранения.</param>
    private static async Task SaveLogToFileAsync(string logMessage)
    {
        try
        {
            // Создаем директорию хранения логов.
            CreateLogDirectory();

            // Создаем файл логов при отсутствии.
            CreateLogFile();

            // Проверяем размер файла.
            CheckLogFileSize();

            // Открываем файл для добавления записи.
            using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
            {
                await writer.WriteLineAsync(logMessage);
            }
        }
        catch (Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError($"Failed to write log file: {exception.Message}");
#endif
        }
    }


    /// <summary>
    /// Метод создает директорию хранения логов.
    /// </summary>
    private static void CreateLogDirectory()
    {
        // Получаем путь директории.
        string directoryPath = Path.GetDirectoryName(_logFilePath);

        // Создаем директорию при отсутствии.
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }


    /// <summary>
    /// Метод создает файл логов при отсутствии.
    /// </summary>
    private static void CreateLogFile()
    {
        // Проверяем существование файла.
        if (File.Exists(_logFilePath))
        {
            return;
        }

        // Создаем пустой файл.
        using (File.Create(_logFilePath))
        {
        }
    }


    /// <summary>
    /// Метод проверяет размер файла логов.
    /// </summary>
    private static void CheckLogFileSize()
    {
        // Получаем информацию о файле.
        FileInfo fileInfo = new FileInfo(_logFilePath);

        // Проверяем превышение максимального размера.
        if (fileInfo.Length <= Constants.MaxLogFileSize)
        {
            return;
        }

        // Удаляем старый файл.
        File.Delete(_logFilePath);

        // Создаем новый файл.
        CreateLogFile();
    }

    #endregion
}