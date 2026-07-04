using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public static class Settings
{
    // Путь к файлу настроек
    private static readonly string _settingsFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "Valley of Elves",
        "settings.json"
    );

    // Регулярное выражение для проверки формата разрешения экрана
    private static readonly Regex _resolutionRegex = new Regex(@"^(\d+)x(\d+)$", RegexOptions.Compiled);

    /// <summary>
    /// Класс для хранения данных настроек
    /// </summary>
    [Serializable]
    private class SettingsData
    {
        public string ScreenResolution = "1920x1080";   // Разрешение экрана, по умолчанию 1920x1080
        public string Localization = "English";         // Локализация игры, по умолчанию English
        public string LanguageCode = "EN";              // Код языка, по умолчанию EN
        public bool IsFullScreen = true;                // Значение состояния полноэкранного режима, по умолчанию включен
        public float MusicVolume = 100f;                // Громкость музыки от 0 до 100, по умолчанию 100
        public float SoundVolume = 100f;                // Громкость звуков от 0 до 100, по умолчанию 100
        public bool IsFileLogging = true;               // Значение состояния записи логов в файл, по умолчанию включен
    }

    // Объект для хранения текущих значений настроек
    private static SettingsData _currentSettings = new SettingsData();

    // Свойство для получения и установки разрешения экрана
    public static string ScreenResolution
    {
        get => _currentSettings.ScreenResolution;
        set
        {
            if (IsValidResolution(value))
            {
                _currentSettings.ScreenResolution = value;
            }
            else
            {
                LogError("Invalid screen resolution format! Expected format 'width x height'!");
            }
        }
    }

    // Свойство для получения и установки локализации игры
    public static string Localization
    {
        get => _currentSettings.Localization;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                _currentSettings.Localization = value;
            }
            else
            {
                LogError("Localization cannot be null or empty!");
            }
        }
    }

    // Свойство для получения и установки кода языка
    public static string LanguageCode
    {
        get => _currentSettings.LanguageCode;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                _currentSettings.LanguageCode = value;
            }
            else
            {
                LogError("Language code cannot be null or empty!");
            }
        }
    }

    // Свойство для доступа к состоянию полноэкранного режима
    public static bool IsFullScreen
    {
        get => _currentSettings.IsFullScreen;
        set => _currentSettings.IsFullScreen = value;
    }

    // Свойство для получения и установки громкости музыки (от 0 до 100)
    public static float MusicVolume
    {
        get => _currentSettings.MusicVolume;
        set => _currentSettings.MusicVolume = Mathf.Clamp(value, 0, 100);
    }

    // Свойство для получения и установки громкости звуков (от 0 до 100)
    public static float SoundVolume
    {
        get => _currentSettings.SoundVolume;
        set => _currentSettings.SoundVolume = Mathf.Clamp(value, 0, 100);
    }

    // Свойство для доступа к состоянию записи логов в файл
    public static bool IsFileLogging
    {
        get => _currentSettings.IsFileLogging;
        set => _currentSettings.IsFileLogging = value;
    }

    /// <summary>
    /// Метод для проверки формата разрешения экрана
    /// </summary>
    /// <param name="resolution">Строка разрешения экрана</param>
    /// <returns>True, если формат разрешения корректен; иначе false.</returns>
    private static bool IsValidResolution(string resolution)
    {
        return !string.IsNullOrWhiteSpace(resolution) && _resolutionRegex.IsMatch(resolution);
    }

    /// <summary>
    /// Метод для сохранения текущих настроек в файл JSON
    /// </summary>
    public static void Save()
    {
        try
        {
            // Создание директории, если она не существует
            Directory.CreateDirectory(Path.GetDirectoryName(_settingsFilePath));
            // Сериализация объекта настроек в строку JSON
            string json = JsonUtility.ToJson(_currentSettings);
            // Запись JSON-строки в файл настроек
            File.WriteAllText(_settingsFilePath, json);
            LogInfo("Settings saved successfully.");
        }
        catch (Exception ex)
        {
            LogError($"Failed to save settings: {ex.Message}!");
        }
    }

    /// <summary>
    /// Метод для загрузки настроек из файла JSON
    /// </summary>
    public static void Load()
    {
        try
        {
            if (File.Exists(_settingsFilePath))
            {
                // Чтение содержимого файла настроек в формате JSON
                string json = File.ReadAllText(_settingsFilePath);
                // Десериализация строки JSON в объект настроек
                _currentSettings = JsonUtility.FromJson<SettingsData>(json) ?? new SettingsData();
                LogInfo("Settings loaded successfully.");
            }
            else
            {
                // Установка значений по умолчанию и создание файла, если он отсутствует
                SetDefaultSettings();
                Save();
                LogWarning("Settings file not found. Created a new one with default values!");
            }
        }
        catch (Exception ex)
        {
            LogError($"Failed to load settings: {ex.Message}!");
        }
    }

    /// <summary>
    /// Метод для установки настроек по умолчанию
    /// </summary>
    public static void SetDefaultSettings()
    {
        // Создание нового экземпляра настроек со значениями по умолчанию
        _currentSettings = new SettingsData();
        LogInfo("Default settings applied.");
    }

    /// <summary>
    /// Метод для применения разрешения экрана
    /// </summary>
    public static void ApplyScreenResolution()
    {
        string[] dimensions = ScreenResolution.Split('x'); // Разделяем строку на ширину и высоту
        int width = int.Parse(dimensions[0]); // Преобразуем ширину в целое число
        int height = int.Parse(dimensions[1]); // Преобразуем высоту в целое число
        Screen.SetResolution(width, height, Screen.fullScreen); // Устанавливаем новое разрешение экрана
        LogInfo($"Screen resolution set to: {width}x{height}.");
    }

    /// <summary>
    /// Метод для применения локализации игры
    /// </summary>
    /// <param name="words">Текстовые поля для отображения слов в игре</param>
    public static void ApplyLocalization(Dictionary<string, TMP_Text> words)
    {
        // Загружаем локализацию из файла для текущего языка
        Translator.LoadLocalization(LanguageCode);

        // Заполняем текстовые поля переведенными словами
        foreach (var word in words)
        {
            string keyWord = word.Key;
            TMP_Text wordText = word.Value;

            // Получаем перевод слова
            string translated = Translator.Translation(keyWord);
            wordText.text = translated;
        }

        LogInfo($"Localization set to: {Localization} ({LanguageCode}).");
    }

    /// <summary>
    /// Метод для применения состояния полноэкранного режима
    /// </summary>
    public static void ApplyFullScreen()
    {
        Screen.fullScreen = IsFullScreen; // Устанавливаем полноэкранный режим
        LogInfo($"Full screen mode applied: {IsFullScreen}.");
    }

    /// <summary>
    /// Метод для применения громкости музыки
    /// </summary>
    /// <param name="musicSource">Источник звука для музыки</param>
    public static void ApplyMusicVolume(AudioSource musicSource)
    {
        if (musicSource == null)
        {
            LogWarning("AudioSource for music is null. Cannot apply music volume!");
            return;
        }

        float volume = GetNormalizedMusicVolume();
        musicSource.volume = volume; // Устанавливаем громкость музыки
        LogInfo($"Music volume applied: {MusicVolume} ({volume * 100}%).");
    }

    // <summary>
    /// Метод для применения громкости звуков
    /// </summary>
    /// <param name="soundSource">Источник звука для звуков</param>
    public static void ApplySoundVolume(AudioSource soundSource)
    {
        if (soundSource == null)
        {
            LogWarning("AudioSource for sound is null. Cannot apply sound volume!");
            return;
        }

        float volume = GetNormalizedSoundVolume();
        soundSource.volume = volume; // Устанавливаем громкость звуков
        LogInfo($"Sound volume applied: {SoundVolume} ({volume * 100}%).");
    }

    /// <summary>
    /// Метод для получения нормализованного значение громкости музыки (0 - 1)
    /// </summary>
    /// <returns>Нормализованная громкость музыки</returns>
    private static float GetNormalizedMusicVolume()
    {
        return Mathf.Clamp(MusicVolume / 100f, 0f, 1f); // Ограничиваем значение от 0 до 1
    }

    /// <summary>
    /// Метод для получения нормализованного значение громкости звуков (0 - 1)
    /// </summary>
    /// <returns>Нормализованная громкость звуков</returns>
    private static float GetNormalizedSoundVolume()
    {
        return Mathf.Clamp(SoundVolume / 100f, 0f, 1f); // Ограничиваем значение от 0 до 1
    }

    /// <summary>
    /// Метод для применения состояния записи логов в файл
    /// </summary>
    public static void ApplyFileLogging()
    {
        if (!IsFileLogging)
        {
            Logger.DeleteLogFile(); // Удаляем файл логов
            LogInfo("File logging has been disabled. The log file has been deleted.");
        }
        else
        {
            LogInfo("File logging is enabled. Logs will now be saved to a file.");
        }
    }

    /// <summary>
    /// Метод для логирования ошибок
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private static void LogError(string message)
    {
        Logger.Log(Logger.LogLevel.Error, nameof(Settings), message);
    }

    /// <summary>
    /// Метод для логирования предупреждений
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private static void LogWarning(string message)
    {
        Logger.Log(Logger.LogLevel.Warning, nameof(Settings), message);
    }

    /// <summary>
    /// Метод для логирования информационных сообщений
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private static void LogInfo(string message)
    {
        Logger.Log(Logger.LogLevel.Info, nameof(Settings), message);
    }
}
