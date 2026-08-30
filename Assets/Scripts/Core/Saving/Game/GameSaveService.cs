using System;
using UnityEngine;

/// <summary>
/// Сервис управления сохранением игры.
/// Отвечает за загрузку, сохранение и удаление состояния игры.
/// </summary>
public static class GameSaveService
{
    /// <summary>
    /// Репозиторий для работы с файлом сохранения игры.
    /// </summary>
    private static readonly GameSaveFileRepository _gameSaveFileRepository =
        new GameSaveFileRepository(Globals.GameSaveFilePath);

    /// <summary>
    /// Данные сохранения игры, хранящиеся в памяти.
    /// </summary>
    private static GameSaveData _gameSaveData = new GameSaveData();



    /// <summary>
    /// Признак того, что файл сохранения игры существует.
    /// </summary>
    public static bool IsGameSaveFileExists => _gameSaveFileRepository.Exists();



    /// <summary>
    /// Свойство получает или устанавливает версию сохранения игры.
    /// </summary>
    public static int Version
    {
        get => _gameSaveData.Version;
        set => _gameSaveData.Version = Mathf.Max(1, value);
    }

    /// <summary>
    /// Свойство получает или устанавливает название сцены.
    /// </summary>
    public static string SceneName
    {
        get => _gameSaveData.SceneName;
        set => _gameSaveData.SceneName = value?.Trim() ?? _gameSaveData.SceneName;
    }



    /// <summary>
    /// Метод сохраняет текущее состояние игры в файл.
    /// </summary>
    public static void Save()
    {
        try
        {
            GameSaveDataValidator.Normalize(_gameSaveData);

            string json = JsonUtility.ToJson(_gameSaveData, true);

            if (string.IsNullOrWhiteSpace(json))
            {
                LogWarning("Game save serialization returned empty JSON. Save skipped.");
                return;
            }

            _gameSaveFileRepository.Save(json);

            LogInfo("Game save saved successfully.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to save game save: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод загружает сохранение игры из файла.
    /// </summary>
    /// <returns>True, если сохранение игры загружено успешно; иначе false.</returns>
    public static bool Load()
    {
        try
        {
            if (!_gameSaveFileRepository.Exists())
            {
                LogWarning("Game save file not found.");

                return false;
            }

            string json = _gameSaveFileRepository.Load();

            if (string.IsNullOrWhiteSpace(json))
            {
                LogWarning("Game save file is empty.");

                return false;
            }

            GameSaveData loaded = JsonUtility.FromJson<GameSaveData>(json);

            if (loaded == null)
            {
                LogWarning("Failed to deserialize game save.");

                return false;
            }

            _gameSaveData = loaded;

            GameSaveDataValidator.Normalize(_gameSaveData);

            LogInfo("Game save loaded successfully.");

            return true;
        }
        catch (Exception exception)
        {
            LogError($"Failed to load game save: {exception.Message}");

            return false;
        }
    }

    /// <summary>
    /// Метод сбрасывает сохранение игры к значениям по умолчанию.
    /// </summary>
    public static void Default()
    {
        _gameSaveData = new GameSaveData();
        LogInfo("Game save reset to default values.");
    }

    /// <summary>
    /// Метод удаляет файл сохранения игры.
    /// </summary>
    public static void Delete()
    {
        try
        {
            _gameSaveFileRepository.Delete();
            LogInfo("Game save deleted successfully.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to delete game save: {exception.Message}");
        }
    }



    /// <summary>
    /// Метод записывает информационное сообщение в лог.
    /// </summary>
    private static void LogInfo(string message)
    {
        Logger.Log(LogLevel.Info, nameof(GameSaveService), message);
    }

    /// <summary>
    /// Метод записывает предупреждение в лог.
    /// </summary>
    private static void LogWarning(string message)
    {
        Logger.Log(LogLevel.Warning, nameof(GameSaveService), message);
    }

    /// <summary>
    /// Метод записывает ошибку в лог.
    /// </summary>
    private static void LogError(string message)
    {
        Logger.Log(LogLevel.Error, nameof(GameSaveService), message);
    }
}