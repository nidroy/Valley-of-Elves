using System;
using System.IO;

/// <summary>
/// Репозиторий для работы с файлом сохранения игры.
/// Отвечает только за чтение, запись и удаление JSON-файла на диске.
/// </summary>
public class GameSaveFileRepository
{
    /// <summary>
    /// Полный путь к файлу сохранения игры.
    /// </summary>
    private readonly string _gameSaveFilePath;



    /// <summary>
    /// Конструктор создаёт новый репозиторий для работы с файлом сохранения игры.
    /// </summary>
    /// <param name="gameSaveFilePath">Путь к файлу сохранения игры.</param>
    public GameSaveFileRepository(string gameSaveFilePath)
    {
        if (string.IsNullOrWhiteSpace(gameSaveFilePath))
        {
            throw new ArgumentException("Game save file path cannot be empty.", nameof(gameSaveFilePath));
        }

        _gameSaveFilePath = gameSaveFilePath;

        CreateGameSaveFileDirectory();

        LogInfo($"GameSaveFileRepository initialized. Path: {_gameSaveFilePath}");
    }



    /// <summary>
    /// Метод проверяет, существует ли файл сохранения игры.
    /// </summary>
    /// <returns>True, если файл сохранения существует, иначе false.</returns>
    public bool Exists()
    {
        try
        {
            bool exists = File.Exists(_gameSaveFilePath);

            LogInfo($"Game save file existence checked. Exists: {exists}");
            return exists;
        }
        catch (Exception exception)
        {
            LogError($"Failed to check game save file existence. Path: {_gameSaveFilePath}. Error: {exception.Message}");
            return false;
        }
    }



    /// <summary>
    /// Метод сохраняет JSON-строку в файл сохранения игры.
    /// </summary>
    /// <param name="json">JSON-строка с данными сохранения.</param>
    public void Save(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            LogWarning("Game save skipped because JSON is empty.");
            return;
        }

        try
        {
            File.WriteAllText(_gameSaveFilePath, json);

            LogInfo($"Game save file written successfully. Path: {_gameSaveFilePath}");
        }
        catch (Exception exception)
        {
            LogError($"Failed to game save file. Path: {_gameSaveFilePath}. Error: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод загружает JSON-строку из файла сохранения игры.
    /// </summary>
    /// <returns>JSON-строка, либо null если загрузка не удалась.</returns>
    public string Load()
    {
        try
        {
            if (!File.Exists(_gameSaveFilePath))
            {
                LogWarning($"Game save file not found. Path: {_gameSaveFilePath}");
                return null;
            }

            string json = File.ReadAllText(_gameSaveFilePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                LogWarning($"Game save file is empty. Path: {_gameSaveFilePath}");
                return null;
            }

            LogInfo($"Game save file loaded successfully. Path: {_gameSaveFilePath}");
            return json;
        }
        catch (Exception exception)
        {
            LogError($"Failed to load game save file. Path: {_gameSaveFilePath}. Error: {exception.Message}");
            return null;
        }
    }

    /// <summary>
    /// Метод удаляет файл сохранения игры, если он существует.
    /// </summary>
    public void Delete()
    {
        try
        {
            if (!File.Exists(_gameSaveFilePath))
            {
                LogWarning($"Game save file not found for delete. Path: {_gameSaveFilePath}");
                return;
            }

            File.Delete(_gameSaveFilePath);

            LogInfo($"Game save file deleted successfully. Path: {_gameSaveFilePath}");
        }
        catch (Exception exception)
        {
            LogError($"Failed to delete game save file. Path: {_gameSaveFilePath}. Error: {exception.Message}");
        }
    }



    /// <summary>
    /// Метод создаёт директорию для файла сохранения игры, если она ещё не существует.
    /// </summary>
    private void CreateGameSaveFileDirectory()
    {
        try
        {
            string directory = Path.GetDirectoryName(_gameSaveFilePath);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);

                LogInfo($"Game save directory ensured. Directory: {directory}");
            }
        }
        catch (Exception exception)
        {
            LogError($"Failed to create game save directory. Path: {_gameSaveFilePath}. Error: {exception.Message}");
        }
    }



    /// <summary>
    /// Метод записывает информационное сообщение в лог.
    /// </summary>
    private static void LogInfo(string message)
    {
        Logger.Log(LogLevel.Info, nameof(GameSaveFileRepository), message);
    }

    /// <summary>
    /// Метод записывает предупреждение в лог.
    /// </summary>
    private static void LogWarning(string message)
    {
        Logger.Log(LogLevel.Warning, nameof(GameSaveFileRepository), message);
    }

    /// <summary>
    /// Метод записывает ошибку в лог.
    /// </summary>
    private static void LogError(string message)
    {
        Logger.Log(LogLevel.Error, nameof(GameSaveFileRepository), message);
    }
}