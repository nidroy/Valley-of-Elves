using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

/// <summary>
/// Класс управляет настройками игры.
/// </summary>
public static class Settings
{
    #region Приватные классы

    /// <summary>
    /// Класс хранит константы настроек.
    /// </summary>
    private static class Constants
    {
        // Название папки игры для хранения данных.
        public const string GameFolderName = "Valley of Elves";

        // Название файла настроек.
        public const string SettingsFileName = "settings.json";

        // Минимальное значение громкости.
        public const float MinVolume = 0f;

        // Максимальное значение громкости.
        public const float MaxVolume = 100f;

        // Формат проверки разрешения экрана.
        public const string ResolutionPattern = @"^(\d+)x(\d+)$";
    }

    /// <summary>
    /// Класс хранит значения настроек по умолчанию.
    /// </summary>
    private static class DefaultSettings
    {
        // Разрешение экрана по умолчанию.
        public const string ScreenResolution = "1920x1080";

        // Локализация по умолчанию.
        public const string Localization = "English";

        // Код языка по умолчанию.
        public const string LanguageCode = "EN";

        // Полноэкранный режим по умолчанию.
        public const bool IsFullScreen = true;

        // Громкость музыки по умолчанию.
        public const float MusicVolume = 100f;

        // Громкость звуков по умолчанию.
        public const float SoundVolume = 100f;

        // Запись логов в файл по умолчанию.
        public const bool IsFileLogging = true;
    }

    /// <summary>
    /// Класс хранит данные настроек для сериализации.
    /// </summary>
    [Serializable]
    private class SettingsData
    {
        // Разрешение экрана.
        public string ScreenResolution = DefaultSettings.ScreenResolution;

        // Название локализации.
        public string Localization = DefaultSettings.Localization;

        // Код языка.
        public string LanguageCode = DefaultSettings.LanguageCode;

        // Состояние полноэкранного режима.
        public bool IsFullScreen = DefaultSettings.IsFullScreen;

        // Громкость музыки.
        public float MusicVolume = DefaultSettings.MusicVolume;

        // Громкость звуков.
        public float SoundVolume = DefaultSettings.SoundVolume;

        // Состояние записи логов в файл.
        public bool IsFileLogging = DefaultSettings.IsFileLogging;
    }

    #endregion


    #region Приватные поля

    // Путь к файлу настроек.
    private static readonly string _settingsFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        Constants.GameFolderName,
        Constants.SettingsFileName
    );

    // Регулярное выражение для проверки разрешения экрана.
    private static readonly Regex _resolutionRegex = new Regex(
        Constants.ResolutionPattern,
        RegexOptions.Compiled
    );

    // Текущие настройки игры.
    private static SettingsData _currentSettings = CreateDefaultSettings();

    #endregion


    #region Публичные свойства

    /// <summary>
    /// Свойство получает или устанавливает разрешение экрана.
    /// </summary>
    public static string ScreenResolution
    {
        get => _currentSettings.ScreenResolution;

        set
        {
            if (!ValidateResolution(value))
            {
                LogError("Invalid screen resolution format.");
                return;
            }

            _currentSettings.ScreenResolution = value;
        }
    }

    /// <summary>
    /// Свойство получает или устанавливает название локализации.
    /// </summary>
    public static string Localization
    {
        get => _currentSettings.Localization;

        set
        {
            if (!ValidateString(value))
            {
                LogError("Localization cannot be null or empty.");
                return;
            }

            _currentSettings.Localization = value;
        }
    }

    /// <summary>
    /// Свойство получает или устанавливает код языка.
    /// </summary>
    public static string LanguageCode
    {
        get => _currentSettings.LanguageCode;

        set
        {
            if (!ValidateString(value))
            {
                LogError("Language code cannot be null or empty.");
                return;
            }

            _currentSettings.LanguageCode = value;
        }
    }

    /// <summary>
    /// Свойство получает или устанавливает состояние полноэкранного режима.
    /// </summary>
    public static bool IsFullScreen
    {
        get => _currentSettings.IsFullScreen;

        set => _currentSettings.IsFullScreen = value;
    }

    /// <summary>
    /// Свойство получает или устанавливает громкость музыки.
    /// </summary>
    public static float MusicVolume
    {
        get => _currentSettings.MusicVolume;

        set => _currentSettings.MusicVolume = Mathf.Clamp(value, Constants.MinVolume, Constants.MaxVolume);
    }

    /// <summary>
    /// Свойство получает или устанавливает громкость звуков.
    /// </summary>
    public static float SoundVolume
    {
        get => _currentSettings.SoundVolume;

        set => _currentSettings.SoundVolume = Mathf.Clamp(value, Constants.MinVolume, Constants.MaxVolume);
    }

    /// <summary>
    /// Свойство получает или устанавливает состояние записи логов в файл.
    /// </summary>
    public static bool IsFileLogging
    {
        get => _currentSettings.IsFileLogging;

        set => _currentSettings.IsFileLogging = value;
    }

    #endregion


    #region Публичные методы

    /// <summary>
    /// Метод сохраняет текущие настройки в файл.
    /// </summary>
    public static void Save()
    {
        try
        {
            // Проверяем корректность текущих настроек.
            ValidateSettings();

            // Создаем директорию настроек.
            CreateSettingsDirectory();

            // Преобразуем настройки в JSON.
            string json = JsonUtility.ToJson(_currentSettings, true);

            // Сохраняем JSON в файл.
            File.WriteAllText(_settingsFilePath, json);

            LogInfo("Settings saved successfully.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to save settings: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод загружает настройки из файла.
    /// </summary>
    public static void Load()
    {
        try
        {
            // Проверяем наличие файла настроек.
            if (!SettingsFileExists())
            {
                // Устанавливаем настройки по умолчанию.
                SetDefaultSettings();

                // Создаем новый файл настроек.
                Save();

                LogWarning("Settings file not found. Default settings created.");

                return;
            }

            // Загружаем настройки из существующего файла.
            LoadSettingsFromFile();

            LogInfo("Settings loaded successfully.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to load settings: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод устанавливает настройки по умолчанию.
    /// </summary>
    public static void SetDefaultSettings()
    {
        // Создаем новый объект настроек.
        _currentSettings = CreateDefaultSettings();

        LogInfo("Default settings applied.");
    }

    /// <summary>
    /// Метод применяет разрешение экрана.
    /// </summary>
    public static void ApplyScreenResolution()
    {
        try
        {
            // Разделяем разрешение на ширину и высоту.
            string[] dimensions = ScreenResolution.Split('x');

            // Получаем ширину экрана.
            int width = int.Parse(dimensions[0]);

            // Получаем высоту экрана.
            int height = int.Parse(dimensions[1]);

            // Устанавливаем разрешение.
            Screen.SetResolution(width, height, Screen.fullScreen);

            LogInfo($"Screen resolution applied: {ScreenResolution}.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to apply screen resolution: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод применяет выбранную локализацию.
    /// </summary>
    /// <param name="words">Словарь элементов интерфейса и их ключей.</param>
    public static void ApplyLocalization(Dictionary<string, TMP_Text> words)
    {
        try
        {
            // Проверяем наличие элементов интерфейса.
            if (words == null || words.Count == 0)
            {
                LogWarning("Localization words collection is empty.");
                return;
            }

            // Загружаем файл локализации.
            Translator.LoadLocalization(LanguageCode);

            // Обновляем текстовые элементы.
            foreach (KeyValuePair<string, TMP_Text> word in words)
            {
                // Получаем ключ перевода.
                string key = word.Key;

                // Получаем текстовый компонент.
                TMP_Text text = word.Value;

                // Проверяем наличие компонента.
                if (text == null)
                {
                    LogWarning($"Text component for key '{key}' is null.");
                    continue;
                }

                // Устанавливаем перевод.
                text.text = Translator.Translation(key);
            }

            LogInfo($"Localization applied: {Localization}.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to apply localization: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод применяет состояние полноэкранного режима.
    /// </summary>
    public static void ApplyFullScreen()
    {
        try
        {
            // Устанавливаем состояние окна.
            Screen.fullScreen = IsFullScreen;

            LogInfo($"Fullscreen mode applied: {IsFullScreen}.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to apply fullscreen mode: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод применяет громкость музыки.
    /// </summary>
    /// <param name="musicSource">Источник воспроизведения музыки.</param>
    public static void ApplyMusicVolume(AudioSource musicSource)
    {
        try
        {
            // Проверяем наличие источника музыки.
            if (musicSource == null)
            {
                LogWarning("Music AudioSource is null.");
                return;
            }

            // Получаем нормализованное значение громкости.
            float volume = GetNormalizedMusicVolume();

            // Применяем громкость к источнику.
            musicSource.volume = volume;

            LogInfo($"Music volume applied: {MusicVolume}%.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to apply music volume: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод применяет громкость звуков.
    /// </summary>
    /// <param name="soundSource">Источник воспроизведения звука.</param>
    public static void ApplySoundVolume(AudioSource soundSource)
    {
        try
        {
            // Проверяем наличие источника звука.
            if (soundSource == null)
            {
                LogWarning("Sound AudioSource is null.");
                return;
            }

            // Получаем нормализованное значение громкости.
            float volume = GetNormalizedSoundVolume();

            // Применяем громкость к источнику.
            soundSource.volume = volume;

            LogInfo($"Sound volume applied: {SoundVolume}%.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to apply sound volume: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод применяет состояние записи логов в файл.
    /// </summary>
    public static void ApplyFileLogging()
    {
        try
        {
            // Передаем состояние записи логов в Logger.
            Logger.IsFileLoggingEnabled = IsFileLogging;

            // Проверяем отключение записи логов.
            if (!IsFileLogging)
            {
                Logger.DeleteLogFile();

                LogInfo("File logging disabled. Log file deleted.");
                return;
            }

            LogInfo("File logging enabled.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to apply file logging: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод проверяет существование файла настроек.
    /// </summary>
    /// <returns>True, если файл настроек существует.</returns>
    public static bool SettingsFileExists()
    {
        // Проверяем существование файла.
        return File.Exists(_settingsFilePath);
    }

    #endregion


    #region Создание объектов

    /// <summary>
    /// Метод создает настройки со значениями по умолчанию.
    /// </summary>
    /// <returns>Объект настроек по умолчанию.</returns>
    private static SettingsData CreateDefaultSettings()
    {
        // Создаем новый объект настроек.
        SettingsData settings = new SettingsData();

        // Возвращаем созданные настройки.
        return settings;
    }

    /// <summary>
    /// Метод создает директорию для файла настроек.
    /// </summary>
    private static void CreateSettingsDirectory()
    {
        // Получаем путь к директории настроек.
        string directoryPath = Path.GetDirectoryName(_settingsFilePath);

        // Проверяем корректность пути.
        if (string.IsNullOrWhiteSpace(directoryPath))
        {
            LogError("Settings directory path is invalid.");
            return;
        }

        // Создаем директорию при отсутствии.
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    #endregion


    #region Проверка настроек

    /// <summary>
    /// Метод проверяет корректность всех настроек.
    /// </summary>
    /// <returns>True, если все настройки корректны.</returns>
    private static bool ValidateSettings()
    {
        // Флаг результата проверки.
        bool isValid = true;


        // Проверяем разрешение экрана.
        if (!ValidateResolution(_currentSettings.ScreenResolution))
        {
            _currentSettings.ScreenResolution = DefaultSettings.ScreenResolution;

            isValid = false;

            LogWarning("Invalid screen resolution. Default value restored.");
        }


        // Проверяем локализацию.
        if (!ValidateString(_currentSettings.Localization))
        {
            _currentSettings.Localization = DefaultSettings.Localization;

            isValid = false;

            LogWarning("Invalid localization. Default value restored.");
        }


        // Проверяем код языка.
        if (!ValidateString(_currentSettings.LanguageCode))
        {
            _currentSettings.LanguageCode = DefaultSettings.LanguageCode;

            isValid = false;

            LogWarning("Invalid language code. Default value restored.");
        }


        // Ограничиваем громкость музыки.
        _currentSettings.MusicVolume = Mathf.Clamp(
            _currentSettings.MusicVolume,
            Constants.MinVolume,
            Constants.MaxVolume
        );


        // Ограничиваем громкость звуков.
        _currentSettings.SoundVolume = Mathf.Clamp(
            _currentSettings.SoundVolume,
            Constants.MinVolume,
            Constants.MaxVolume
        );


        return isValid;
    }

    /// <summary>
    /// Метод проверяет формат разрешения экрана.
    /// </summary>
    /// <param name="resolution">Разрешение экрана.</param>
    /// <returns>True, если разрешение корректное.</returns>
    private static bool ValidateResolution(string resolution)
    {
        // Проверяем строку регулярным выражением.
        return !string.IsNullOrWhiteSpace(resolution) &&
               _resolutionRegex.IsMatch(resolution);
    }

    /// <summary>
    /// Метод проверяет строковое значение.
    /// </summary>
    /// <param name="value">Проверяемая строка.</param>
    /// <returns>True, если строка содержит значение.</returns>
    private static bool ValidateString(string value)
    {
        // Проверяем наличие текста.
        return !string.IsNullOrWhiteSpace(value);
    }

    #endregion


    #region Вспомогательные методы

    /// <summary>
    /// Метод загружает настройки из файла.
    /// </summary>
    private static void LoadSettingsFromFile()
    {
        // Читаем содержимое файла настроек.
        string json = File.ReadAllText(_settingsFilePath);

        // Преобразуем JSON в объект настроек.
        _currentSettings = JsonUtility.FromJson<SettingsData>(json) ?? new SettingsData();

        LogInfo("Settings loaded from file.");
    }

    /// <summary>
    /// Метод получает нормализованное значение громкости музыки.
    /// </summary>
    /// <returns>Значение громкости от 0 до 1.</returns>
    private static float GetNormalizedMusicVolume()
    {
        // Преобразуем процентное значение громкости в диапазон 0-1.
        return Mathf.Clamp(
            MusicVolume / Constants.MaxVolume,
            0f,
            1f
        );
    }

    /// <summary>
    /// Метод получает нормализованное значение громкости звуков.
    /// </summary>
    /// <returns>Значение громкости от 0 до 1.</returns>
    private static float GetNormalizedSoundVolume()
    {
        // Преобразуем процентное значение громкости в диапазон 0-1.
        return Mathf.Clamp(
            SoundVolume / Constants.MaxVolume,
            0f,
            1f
        );
    }

    #endregion


    #region Логирование

    /// <summary>
    /// Метод записывает информационное сообщение.
    /// </summary>
    /// <param name="message">Сообщение для записи.</param>
    private static void LogInfo(string message)
    {
        Logger.Log(Logger.LogLevel.Info, nameof(Settings), message);
    }

    /// <summary>
    /// Метод записывает предупреждение.
    /// </summary>
    /// <param name="message">Сообщение предупреждения.</param>
    private static void LogWarning(string message)
    {
        Logger.Log(Logger.LogLevel.Warning, nameof(Settings), message);
    }

    /// <summary>
    /// Метод записывает ошибку.
    /// </summary>
    /// <param name="message">Сообщение ошибки.</param>
    private static void LogError(string message)
    {
        Logger.Log(Logger.LogLevel.Error, nameof(Settings), message);
    }

    #endregion
}