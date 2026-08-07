using System;
using System.IO;

/// <summary>
/// Репозиторий для работы с файлом сохранения игрока.
/// Отвечает только за чтение, запись и удаление JSON-файла на диске.
/// </summary>
public class PlayerSaveFileRepository
{
    /// <summary>
    /// Полный путь к файлу сохранения игрока.
    /// </summary>
    private readonly string _playerSaveFilePath;



    /// <summary>
    /// Конструктор создаёт новый репозиторий для работы с файлом сохранения игрока.
    /// </summary>
    /// <param name="playerSaveFilePath">Путь к файлу сохранения игрока.</param>
    public PlayerSaveFileRepository(string playerSaveFilePath)
    {
        if (string.IsNullOrWhiteSpace(playerSaveFilePath))
        {
            throw new ArgumentException("Player save file path cannot be empty.", nameof(playerSaveFilePath));
        }

        _playerSaveFilePath = playerSaveFilePath;

        CreatePlayerSaveFileDirectory();

        LogInfo($"PlayerSaveFileRepository initialized. Path: {_playerSaveFilePath}");
    }



    /// <summary>
    /// Метод проверяет, существует ли файл сохранения игрока.
    /// </summary>
    /// <returns>True, если файл сохранения существует, иначе false.</returns>
    public bool Exists()
    {
        try
        {
            bool exists = File.Exists(_playerSaveFilePath);

            LogInfo($"Player save file existence checked. Exists: {exists}");
            return exists;
        }
        catch (Exception exception)
        {
            LogError($"Failed to check player save file existence. Path: {_playerSaveFilePath}. Error: {exception.Message}");
            return false;
        }
    }



    /// <summary>
    /// Метод сохраняет JSON-строку в файл сохранения игрока.
    /// </summary>
    /// <param name="json">JSON-строка с данными сохранения.</param>
    public void Save(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            LogWarning("Player save skipped because JSON is empty.");
            return;
        }

        try
        {
            File.WriteAllText(_playerSaveFilePath, json);

            LogInfo($"Player save file written successfully. Path: {_playerSaveFilePath}");
        }
        catch (Exception exception)
        {
            LogError($"Failed to player save file. Path: {_playerSaveFilePath}. Error: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод загружает JSON-строку из файла сохранения игрока.
    /// </summary>
    /// <returns>JSON-строка, либо null если загрузка не удалась.</returns>
    public string Load()
    {
        try
        {
            if (!File.Exists(_playerSaveFilePath))
            {
                LogWarning($"Player save file not found. Path: {_playerSaveFilePath}");
                return null;
            }

            string json = File.ReadAllText(_playerSaveFilePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                LogWarning($"Player save file is empty. Path: {_playerSaveFilePath}");
                return null;
            }

            LogInfo($"Player save file loaded successfully. Path: {_playerSaveFilePath}");
            return json;
        }
        catch (Exception exception)
        {
            LogError($"Failed to load player save file. Path: {_playerSaveFilePath}. Error: {exception.Message}");
            return null;
        }
    }

    /// <summary>
    /// Метод удаляет файл сохранения игрока, если он существует.
    /// </summary>
    public void Delete()
    {
        try
        {
            if (!File.Exists(_playerSaveFilePath))
            {
                LogWarning($"Player save file not found for delete. Path: {_playerSaveFilePath}");
                return;
            }

            File.Delete(_playerSaveFilePath);

            LogInfo($"Player save file deleted successfully. Path: {_playerSaveFilePath}");
        }
        catch (Exception exception)
        {
            LogError($"Failed to delete player save file. Path: {_playerSaveFilePath}. Error: {exception.Message}");
        }
    }



    /// <summary>
    /// Метод создаёт директорию для файла сохранения игрока, если она ещё не существует.
    /// </summary>
    private void CreatePlayerSaveFileDirectory()
    {
        try
        {
            string directory = Path.GetDirectoryName(_playerSaveFilePath);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);

                LogInfo($"Player save directory ensured. Directory: {directory}");
            }
        }
        catch (Exception exception)
        {
            LogError($"Failed to create player save directory. Path: {_playerSaveFilePath}. Error: {exception.Message}");
        }
    }



    /// <summary>
    /// Метод записывает информационное сообщение в лог.
    /// </summary>
    private static void LogInfo(string message)
    {
        Logger.Log(LogLevel.Info, nameof(PlayerSaveFileRepository), message);
    }

    /// <summary>
    /// Метод записывает предупреждение в лог.
    /// </summary>
    private static void LogWarning(string message)
    {
        Logger.Log(LogLevel.Warning, nameof(PlayerSaveFileRepository), message);
    }

    /// <summary>
    /// Метод записывает ошибку в лог.
    /// </summary>
    private static void LogError(string message)
    {
        Logger.Log(LogLevel.Error, nameof(PlayerSaveFileRepository), message);
    }
}