using System;
using System.IO;

/// <summary>
/// Репозиторий для работы с файлом настроек игры.
/// Отвечает только за чтение и запись JSON-файла на диске.
/// </summary>
public class SettingsFileRepository
{
    /// <summary>
    /// Полный путь к файлу настроек игры.
    /// </summary>
    private readonly string _gameSettingsFilePath;



    /// <summary>
    /// Конструктор создаёт новый репозиторий для работы с файлом настроек игры.
    /// </summary>
    /// <param name="gameSettingsFilePath">Путь к файлу настроек игры.</param>
    public SettingsFileRepository(string gameSettingsFilePath)
    {
        if (string.IsNullOrWhiteSpace(gameSettingsFilePath))
        {
            throw new ArgumentException("Game settings file path cannot be empty.", nameof(gameSettingsFilePath));
        }

        _gameSettingsFilePath = gameSettingsFilePath;

        CreateSettingsFileDirectory();

        LogInfo($"SettingsFileRepository initialized. Path: {_gameSettingsFilePath}");
    }



    /// <summary>
    /// Метод проверяет, существует ли файл настроек игры.
    /// </summary>
    /// <returns>True, если файл существует, иначе false.</returns>
    public bool Exists()
    {
        try
        {
            bool exists = File.Exists(_gameSettingsFilePath);

            LogInfo($"Settings file existence checked. Exists: {exists}");
            return exists;
        }
        catch (Exception exception)
        {
            LogError($"Failed to check settings file existence. Path: {_gameSettingsFilePath}. Error: {exception.Message}");
            return false;
        }
    }



    /// <summary>
    /// Метод сохраняет JSON в файл настроек игры.
    /// </summary>
    /// <param name="json">JSON-строка с настройками.</param>
    public void Save(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            LogWarning("Settings save skipped because JSON is empty.");
            return;
        }

        try
        {
            File.WriteAllText(_gameSettingsFilePath, json);

            LogInfo($"Settings saved successfully. Path: {_gameSettingsFilePath}");
        }
        catch (Exception exception)
        {
            LogError($"Failed to save settings file. Path: {_gameSettingsFilePath}. Error: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод загружает JSON из файла настроек игры.
    /// </summary>
    /// <returns>JSON-строка, либо null если загрузка не удалась.</returns>
    public string Load()
    {
        try
        {
            if (!File.Exists(_gameSettingsFilePath))
            {
                LogWarning($"Settings file not found. Path: {_gameSettingsFilePath}");
                return null;
            }

            string json = File.ReadAllText(_gameSettingsFilePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                LogWarning($"Settings file is empty. Path: {_gameSettingsFilePath}");
                return null;
            }

            LogInfo($"Settings loaded successfully. Path: {_gameSettingsFilePath}");
            return json;
        }
        catch (Exception exception)
        {
            LogError($"Failed to load settings file. Path: {_gameSettingsFilePath}. Error: {exception.Message}");
            return null;
        }
    }

    /// <summary>
    /// Метод удаляет файл настроек игры, если он существует.
    /// </summary>
    public void Delete()
    {
        try
        {
            if (!File.Exists(_gameSettingsFilePath))
            {
                LogWarning($"Settings file not found for delete. Path: {_gameSettingsFilePath}");
                return;
            }

            File.Delete(_gameSettingsFilePath);

            LogInfo($"Settings file deleted successfully. Path: {_gameSettingsFilePath}");
        }
        catch (Exception exception)
        {
            LogError($"Failed to delete settings file. Path: {_gameSettingsFilePath}. Error: {exception.Message}");
        }
    }



    /// <summary>
    /// Метод создаёт директорию для файла настроек игры, если она ещё не существует.
    /// </summary>
    private void CreateSettingsFileDirectory()
    {
        try
        {
            string directory = Path.GetDirectoryName(_gameSettingsFilePath);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);

                LogInfo($"Settings directory ensured. Directory: {directory}");
            }
        }
        catch (Exception exception)
        {
            LogError($"Failed to create settings directory. Path: {_gameSettingsFilePath}. Error: {exception.Message}");
        }
    }



    /// <summary>
    /// Метод записывает информационное сообщение в лог.
    /// </summary>
    private void LogInfo(string message)
    {
        Logger.Log(LogLevel.Info, nameof(SettingsFileRepository), message);
    }

    /// <summary>
    /// Метод записывает предупреждение в лог.
    /// </summary>
    private void LogWarning(string message)
    {
        Logger.Log(LogLevel.Warning, nameof(SettingsFileRepository), message);
    }

    /// <summary>
    /// Метод записывает ошибку в лог.
    /// </summary>
    private void LogError(string message)
    {
        Logger.Log(LogLevel.Error, nameof(SettingsFileRepository), message);
    }
}