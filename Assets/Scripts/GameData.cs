using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Класс управляет сохранением и загрузкой игровых данных.
/// </summary>
public static class GameData
{
    #region Приватные константы

    /// <summary>
    /// Класс хранит имена файлов игры.
    /// </summary>
    private static class FileNames
    {
        // Название папки игры.
        public const string GameFolderName = "Valley of Elves";

        // Имя файла сохранения.
        public const string SaveFileName = "save.json";
    }

    /// <summary>
    /// Класс хранит значения игровых данных по умолчанию.
    /// </summary>
    private static class DefaultValues
    {
        // Имя персонажа.
        public const string PlayerName = "";

        // Класс персонажа.
        public const string PlayerClass = "";

        // Уровень персонажа.
        public const int Level = 1;

        // Количество опыта.
        public const int Experience = 0;

        // Текущее здоровье.
        public const int Health = 100;

        // Максимальное здоровье.
        public const int MaxHealth = 100;

        // Текущая мана.
        public const int Mana = 50;

        // Максимальная мана.
        public const int MaxMana = 50;

        // Количество золота.
        public const int Gold = 0;

        // Имя стартовой сцены.
        public const string SceneName = "Game";

        // Начальная координата X.
        public const float PositionX = 0f;

        // Начальная координата Y.
        public const float PositionY = 0f;
    }

    #endregion


    #region Приватные поля

    // Путь к файлу сохранения.
    private static readonly string _saveFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        FileNames.GameFolderName,
        FileNames.SaveFileName);

    // Текущие игровые данные.
    private static SaveData _currentSave = new SaveData();

    #endregion


    #region Внутренние классы

    /// <summary>
    /// Класс хранит игровые данные.
    /// </summary>
    [Serializable]
    private class SaveData
    {
        public string PlayerName = DefaultValues.PlayerName;

        public string PlayerClass = DefaultValues.PlayerClass;

        public int Level = DefaultValues.Level;

        public int Experience = DefaultValues.Experience;

        public int Health = DefaultValues.Health;

        public int MaxHealth = DefaultValues.MaxHealth;

        public int Mana = DefaultValues.Mana;

        public int MaxMana = DefaultValues.MaxMana;

        public int Gold = DefaultValues.Gold;

        public string SceneName = DefaultValues.SceneName;

        public float PositionX = DefaultValues.PositionX;

        public float PositionY = DefaultValues.PositionY;
    }

    #endregion


    #region Публичные свойства

    /// <summary>
    /// Свойство получает или устанавливает имя персонажа.
    /// </summary>
    public static string PlayerName
    {
        get => _currentSave.PlayerName;

        set
        {
            if (ValidateString(value))
            {
                _currentSave.PlayerName = value.Trim();

                return;
            }

            LogError("Player name cannot be null or empty.");
        }
    }

    /// <summary>
    /// Свойство получает или устанавливает класс персонажа.
    /// </summary>
    public static string PlayerClass
    {
        get => _currentSave.PlayerClass;

        set
        {
            if (ValidateString(value))
            {
                _currentSave.PlayerClass = value.Trim();

                return;
            }

            LogError("Player class cannot be null or empty.");
        }
    }

    /// <summary>
    /// Свойство получает или устанавливает уровень персонажа.
    /// </summary>
    public static int Level
    {
        get => _currentSave.Level;

        set => _currentSave.Level = Mathf.Max(1, value);
    }

    /// <summary>
    /// Свойство получает или устанавливает количество опыта.
    /// </summary>
    public static int Experience
    {
        get => _currentSave.Experience;

        set => _currentSave.Experience = Mathf.Max(0, value);
    }

    /// <summary>
    /// Свойство получает или устанавливает текущее здоровье.
    /// </summary>
    public static int Health
    {
        get => _currentSave.Health;

        set => _currentSave.Health = Mathf.Clamp(value, 0, MaxHealth);
    }

    /// <summary>
    /// Свойство получает или устанавливает максимальное здоровье.
    /// </summary>
    public static int MaxHealth
    {
        get => _currentSave.MaxHealth;

        set => _currentSave.MaxHealth = Mathf.Max(1, value);
    }

    /// <summary>
    /// Свойство получает или устанавливает текущую ману.
    /// </summary>
    public static int Mana
    {
        get => _currentSave.Mana;

        set => _currentSave.Mana = Mathf.Clamp(value, 0, MaxMana);
    }

    /// <summary>
    /// Свойство получает или устанавливает максимальную ману.
    /// </summary>
    public static int MaxMana
    {
        get => _currentSave.MaxMana;

        set => _currentSave.MaxMana = Mathf.Max(0, value);
    }

    /// <summary>
    /// Свойство получает или устанавливает количество золота.
    /// </summary>
    public static int Gold
    {
        get => _currentSave.Gold;

        set => _currentSave.Gold = Mathf.Max(0, value);
    }

    /// <summary>
    /// Свойство получает или устанавливает имя текущей сцены.
    /// </summary>
    public static string SceneName
    {
        get => _currentSave.SceneName;

        set
        {
            if (ValidateString(value))
            {
                _currentSave.SceneName = value.Trim();

                return;
            }

            LogError("Scene name cannot be null or empty.");
        }
    }

    /// <summary>
    /// Свойство получает или устанавливает координату X персонажа.
    /// </summary>
    public static float PositionX
    {
        get => _currentSave.PositionX;

        set => _currentSave.PositionX = value;
    }

    /// <summary>
    /// Свойство получает или устанавливает координату Y персонажа.
    /// </summary>
    public static float PositionY
    {
        get => _currentSave.PositionY;

        set => _currentSave.PositionY = value;
    }

    #endregion


    #region Публичные методы

    /// <summary>
    /// Метод создает новое сохранение.
    /// </summary>
    public static void CreateNewSave()
    {
        try
        {
            // Устанавливаем игровые данные по умолчанию.
            ResetSaveData();

            // Сохраняем новые игровые данные.
            Save();

            LogInfo("New save created.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to create new save: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод загружает сохранение.
    /// </summary>
    /// <returns>True, если сохранение успешно загружено.</returns>
    public static bool LoadSave()
    {
        try
        {
            // Проверяем наличие файла сохранения.
            if (!SaveFileExists())
            {
                LogWarning("Save file not found.");

                return false;
            }

            // Загружаем игровые данные.
            Load();

            LogInfo("Save loaded successfully.");

            return true;
        }
        catch (Exception exception)
        {
            LogError($"Failed to load save: {exception.Message}");

            return false;
        }
    }

    /// <summary>
    /// Метод сохраняет игровые данные.
    /// </summary>
    public static void Save()
    {
        try
        {
            // Создаем директорию сохранений.
            CreateSaveDirectory();

            // Сериализуем игровые данные.
            string json = SerializeSaveData();

            // Записываем данные в файл.
            WriteSaveFile(json);

            LogInfo("Game saved successfully.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to save game: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод загружает игровые данные из файла.
    /// </summary>
    public static void Load()
    {
        try
        {
            // Проверяем существование файла сохранения.
            if (!SaveFileExists())
            {
                LogWarning("Save file not found.");

                return;
            }

            // Читаем содержимое файла.
            string json = ReadSaveFile();

            // Загружаем игровые данные.
            DeserializeSaveData(json);

            LogInfo("Game data loaded.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to load game data: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод удаляет файл сохранения.
    /// </summary>
    public static void DeleteSave()
    {
        try
        {
            // Проверяем существование файла.
            if (!SaveFileExists())
            {
                LogWarning("Save file does not exist.");

                return;
            }

            // Удаляем файл сохранения.
            File.Delete(_saveFilePath);

            LogInfo("Save file deleted.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to delete save file: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод проверяет существование файла сохранения.
    /// </summary>
    /// <returns>True, если файл существует.</returns>
    public static bool SaveFileExists()
    {
        // Проверяем наличие файла сохранения.
        return File.Exists(_saveFilePath);
    }

    #endregion


    #region Приватные методы работы с файлами

    /// <summary>
    /// Метод создает папку для сохранения игры.
    /// </summary>
    private static void CreateSaveDirectory()
    {
        // Получаем путь к директории сохранения.
        string directoryPath = Path.GetDirectoryName(_saveFilePath);

        // Создаем директорию, если она отсутствует.
        Directory.CreateDirectory(directoryPath);
    }

    /// <summary>
    /// Метод записывает данные сохранения в файл.
    /// </summary>
    /// <param name="json">Данные сохранения в формате JSON.</param>
    private static void WriteSaveFile(string json)
    {
        // Записываем JSON данные в файл сохранения.
        File.WriteAllText(_saveFilePath, json);
    }

    /// <summary>
    /// Метод читает данные сохранения из файла.
    /// </summary>
    /// <returns>Строка с данными сохранения.</returns>
    private static string ReadSaveFile()
    {
        // Читаем содержимое файла сохранения.
        return File.ReadAllText(_saveFilePath);
    }

    /// <summary>
    /// Метод преобразует игровые данные в JSON строку.
    /// </summary>
    /// <returns>Игровые данные в формате JSON.</returns>
    private static string SerializeSaveData()
    {
        // Преобразуем объект сохранения в JSON.
        return JsonUtility.ToJson(_currentSave, true);
    }

    /// <summary>
    /// Метод загружает игровые данные из JSON строки.
    /// </summary>
    /// <param name="json">JSON строка с игровыми данными.</param>
    private static void DeserializeSaveData(string json)
    {
        // Проверяем наличие данных.
        if (string.IsNullOrWhiteSpace(json))
        {
            LogError("Save data is empty.");

            return;
        }

        // Десериализуем JSON в объект сохранения.
        _currentSave = JsonUtility.FromJson<SaveData>(json);

        // Проверяем результат загрузки.
        if (_currentSave == null)
        {
            _currentSave = new SaveData();

            LogError("Failed to deserialize save data.");
        }
    }

    #endregion


    #region Приватные методы обработки данных

    /// <summary>
    /// Метод сбрасывает игровые данные к значениям по умолчанию.
    /// </summary>
    private static void ResetSaveData()
    {
        // Создаем новый объект сохранения.
        _currentSave = new SaveData();

        LogInfo("Game data reset to default values.");
    }

    /// <summary>
    /// Метод проверяет корректность строки.
    /// </summary>
    /// <param name="value">Проверяемая строка.</param>
    /// <returns>True, если строка содержит значение.</returns>
    private static bool ValidateString(string value)
    {
        // Проверяем, что строка существует и содержит символы.
        return !string.IsNullOrWhiteSpace(value);
    }

    #endregion


    #region Логирование

    /// <summary>
    /// Метод записывает информационное сообщение.
    /// </summary>
    /// <param name="message">Сообщение для записи.</param>
    private static void LogInfo(string message)
    {
        Logger.Log(Logger.LogLevel.Info, nameof(GameData), message);
    }

    /// <summary>
    /// Метод записывает предупреждение.
    /// </summary>
    /// <param name="message">Сообщение предупреждения.</param>
    private static void LogWarning(string message)
    {
        Logger.Log(Logger.LogLevel.Warning, nameof(GameData), message);
    }

    /// <summary>
    /// Метод записывает ошибку.
    /// </summary>
    /// <param name="message">Сообщение ошибки.</param>
    private static void LogError(string message)
    {
        Logger.Log(Logger.LogLevel.Error, nameof(GameData), message);
    }

    #endregion
}