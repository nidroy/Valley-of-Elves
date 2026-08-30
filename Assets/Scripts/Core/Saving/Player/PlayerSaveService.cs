using System;
using UnityEngine;

/// <summary>
/// Сервис управления сохранением игрока.
/// Отвечает за загрузку, сохранение и удаление состояния игрока.
/// </summary>
public static class PlayerSaveService
{
    /// <summary>
    /// Репозиторий для работы с файлом сохранения игрока.
    /// </summary>
    private static readonly PlayerSaveFileRepository _playerSaveFileRepository =
        new PlayerSaveFileRepository(Globals.PlayerSaveFilePath);

    /// <summary>
    /// Данные сохранения игрока, хранящиеся в памяти.
    /// </summary>
    private static PlayerSaveData _playerSaveData = new PlayerSaveData();



    /// <summary>
    /// Признак того, что файл сохранения игрока существует.
    /// </summary>
    public static bool IsPlayerSaveFileExists => _playerSaveFileRepository.Exists();



    /// <summary>
    /// Свойство получает или устанавливает версию сохранения игрока.
    /// </summary>
    public static int Version
    {
        get => _playerSaveData.Version;
        set => _playerSaveData.Version = Mathf.Max(1, value);
    }

    /// <summary>
    /// Свойство получает или устанавливает имя игрока.
    /// </summary>
    public static string Name
    {
        get => _playerSaveData.Name;
        set => _playerSaveData.Name = value?.Trim() ?? _playerSaveData.Name;
    }

    /// <summary>
    /// Свойство получает или устанавливает класс игрока.
    /// </summary>
    public static string Class
    {
        get => _playerSaveData.Class;
        set => _playerSaveData.Class = value?.Trim() ?? _playerSaveData.Class;
    }

    /// <summary>
    /// Свойство получает или устанавливает текст класса игрока.
    /// </summary>
    public static string ClassText
    {
        get => _playerSaveData.ClassText;
        set => _playerSaveData.ClassText = value?.Trim() ?? _playerSaveData.ClassText;
    }

    /// <summary>
    /// Свойство получает или устанавливает уровень игрока.
    /// </summary>
    public static int Level
    {
        get => _playerSaveData.Level;
        set => _playerSaveData.Level = Mathf.Max(1, value);
    }

    /// <summary>
    /// Свойство получает или устанавливает количество опыта игрока.
    /// </summary>
    public static int Experience
    {
        get => _playerSaveData.Experience;
        set => _playerSaveData.Experience = Mathf.Max(0, value);
    }

    /// <summary>
    /// Свойство получает или устанавливает текущее здоровье игрока.
    /// </summary>
    public static int Health
    {
        get => _playerSaveData.Health;
        set => _playerSaveData.Health = Mathf.Max(0, value);
    }

    /// <summary>
    /// Свойство получает или устанавливает максимальное здоровье игрока.
    /// </summary>
    public static int MaxHealth
    {
        get => _playerSaveData.MaxHealth;
        set
        {
            _playerSaveData.MaxHealth = Mathf.Max(1, value);
            _playerSaveData.Health = Mathf.Clamp(_playerSaveData.Health, 0, _playerSaveData.MaxHealth);
        }
    }

    /// <summary>
    /// Свойство получает или устанавливает текущую ману игрока.
    /// </summary>
    public static int Mana
    {
        get => _playerSaveData.Mana;
        set => _playerSaveData.Mana = Mathf.Max(0, value);
    }

    /// <summary>
    /// Свойство получает или устанавливает максимальную ману игрока.
    /// </summary>
    public static int MaxMana
    {
        get => _playerSaveData.MaxMana;
        set
        {
            _playerSaveData.MaxMana = Mathf.Max(0, value);
            _playerSaveData.Mana = Mathf.Clamp(_playerSaveData.Mana, 0, _playerSaveData.MaxMana);
        }
    }

    /// <summary>
    /// Свойство получает или устанавливает количество золота игрока.
    /// </summary>
    public static int Gold
    {
        get => _playerSaveData.Gold;
        set => _playerSaveData.Gold = Mathf.Max(0, value);
    }

    /// <summary>
    /// Свойство получает или устанавливает позицию игрока по оси X.
    /// </summary>
    public static float PositionX
    {
        get => _playerSaveData.PositionX;
        set => _playerSaveData.PositionX = value;
    }

    /// <summary>
    /// Свойство получает или устанавливает позицию игрока по оси Y.
    /// </summary>
    public static float PositionY
    {
        get => _playerSaveData.PositionY;
        set => _playerSaveData.PositionY = value;
    }



    /// <summary>
    /// Метод сохраняет текущее состояние игрока в файл.
    /// </summary>
    public static void Save()
    {
        try
        {
            PlayerSaveDataValidator.Normalize(_playerSaveData);

            string json = JsonUtility.ToJson(_playerSaveData, true);

            if (string.IsNullOrWhiteSpace(json))
            {
                LogWarning("Player save serialization returned empty JSON. Save skipped.");
                return;
            }

            _playerSaveFileRepository.Save(json);

            LogInfo("Player save saved successfully.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to save player save: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод загружает сохранение игрока из файла.
    /// </summary>
    /// <returns>True, если сохранение игрока загружено успешно; иначе false.</returns>
    public static bool Load()
    {
        try
        {
            if (!_playerSaveFileRepository.Exists())
            {
                LogWarning("Player save file not found.");

                return false;
            }

            string json = _playerSaveFileRepository.Load();

            if (string.IsNullOrWhiteSpace(json))
            {
                LogWarning("Player save file is empty.");

                return false;
            }

            PlayerSaveData loaded = JsonUtility.FromJson<PlayerSaveData>(json);

            if (loaded == null)
            {
                LogWarning("Failed to deserialize player save.");

                return false;
            }

            _playerSaveData = loaded;

            PlayerSaveDataValidator.Normalize(_playerSaveData);

            LogInfo("Player save loaded successfully.");

            return true;
        }
        catch (Exception exception)
        {
            LogError($"Failed to load player save: {exception.Message}");

            return false;
        }
    }

    /// <summary>
    /// Метод сбрасывает сохранение игрока к значениям по умолчанию.
    /// </summary>
    public static void Default()
    {
        _playerSaveData = new PlayerSaveData();
        LogInfo("Player save reset to default values.");
    }

    /// <summary>
    /// Метод удаляет файл сохранения игрока.
    /// </summary>
    public static void Delete()
    {
        try
        {
            _playerSaveFileRepository.Delete();
            LogInfo("Player save deleted successfully.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to delete player save: {exception.Message}");
        }
    }



    /// <summary>
    /// Метод записывает информационное сообщение в лог.
    /// </summary>
    private static void LogInfo(string message)
    {
        Logger.Log(LogLevel.Info, nameof(PlayerSaveService), message);
    }

    /// <summary>
    /// Метод записывает предупреждение в лог.
    /// </summary>
    private static void LogWarning(string message)
    {
        Logger.Log(LogLevel.Warning, nameof(PlayerSaveService), message);
    }

    /// <summary>
    /// Метод записывает ошибку в лог.
    /// </summary>
    private static void LogError(string message)
    {
        Logger.Log(LogLevel.Error, nameof(PlayerSaveService), message);
    }
}