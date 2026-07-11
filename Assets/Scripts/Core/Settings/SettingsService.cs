using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

/// <summary>
/// Сервис управления настройками игры.
/// Отвечает за загрузку, сохранение и применение настроек.
/// UI в этом классе не обрабатывается.
/// </summary>
public static class SettingsService
{
    private const string GameFolderName = "Valley of Elves";

    private static readonly SettingsFileRepository Repository =
        new SettingsFileRepository(GameFolderName);

    private static SettingsData _settings = new SettingsData();

    /// <summary>
    /// Текущие настройки в памяти.
    /// </summary>
    public static SettingsData Current => _settings;

    /// <summary>
    /// Получает или устанавливает разрешение экрана.
    /// Формат: "ШиринаxВысота".
    /// </summary>
    public static string ScreenResolution
    {
        get => _settings.ScreenResolution;
        set => _settings.ScreenResolution = value?.Trim() ?? _settings.ScreenResolution;
    }

    /// <summary>
    /// Получает или устанавливает полноэкранный режим.
    /// </summary>
    public static bool IsFullScreen
    {
        get => _settings.IsFullScreen;
        set => _settings.IsFullScreen = value;
    }

    /// <summary>
    /// Получает или устанавливает громкость музыки.
    /// Значение хранится в диапазоне от 0 до 100.
    /// </summary>
    public static float MusicVolume
    {
        get => _settings.MusicVolume;
        set => _settings.MusicVolume = Mathf.Clamp(value, 0f, 100f);
    }

    /// <summary>
    /// Получает или устанавливает громкость звуков.
    /// Значение хранится в диапазоне от 0 до 100.
    /// </summary>
    public static float SoundVolume
    {
        get => _settings.SoundVolume;
        set => _settings.SoundVolume = Mathf.Clamp(value, 0f, 100f);
    }

    /// <summary>
    /// Получает или устанавливает название локализации.
    /// </summary>
    public static string Localization
    {
        get => _settings.Localization;
        set => _settings.Localization = value?.Trim() ?? _settings.Localization;
    }

    /// <summary>
    /// Получает или устанавливает код языка.
    /// </summary>
    public static string LanguageCode
    {
        get => _settings.LanguageCode;
        set => _settings.LanguageCode = value?.Trim() ?? _settings.LanguageCode;
    }

    /// <summary>
    /// Получает или устанавливает запись логов в файл.
    /// </summary>
    public static bool IsFileLogging
    {
        get => _settings.IsFileLogging;
        set => _settings.IsFileLogging = value;
    }

    /// <summary>
    /// Получает или устанавливает уровень качества графики.
    /// </summary>
    public static int QualityLevel
    {
        get => _settings.QualityLevel;
        set => _settings.QualityLevel = Mathf.Clamp(value, 0, Mathf.Max(0, QualitySettings.names.Length - 1));
    }

    /// <summary>
    /// Получает или устанавливает VSync.
    /// </summary>
    public static bool IsVSync
    {
        get => _settings.IsVSync;
        set => _settings.IsVSync = value;
    }

    /// <summary>
    /// Получает или устанавливает ограничение FPS.
    /// </summary>
    public static int FrameRate
    {
        get => _settings.FrameRate;
        set => _settings.FrameRate = Mathf.Max(30, value);
    }

    /// <summary>
    /// Загружает настройки из файла.
    /// Если файл отсутствует или повреждён, загружает значения по умолчанию.
    /// </summary>
    public static void Load()
    {
        try
        {
            if (!Repository.Exists())
            {
                Reset();
                return;
            }

            string json = Repository.Load();
            SettingsData loaded = JsonUtility.FromJson<SettingsData>(json);

            if (loaded == null)
            {
                Reset();
                return;
            }

            _settings = loaded;
            SettingsDataValidator.Normalize(_settings);
            LogInfo("Settings loaded.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to load settings: {exception.Message}");
            Reset();
        }
    }

    /// <summary>
    /// Сохраняет текущие настройки в файл.
    /// </summary>
    public static void Save()
    {
        try
        {
            SettingsDataValidator.Normalize(_settings);
            string json = JsonUtility.ToJson(_settings, true);
            Repository.Save(json);
            LogInfo("Settings saved.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to save settings: {exception.Message}");
        }
    }

    /// <summary>
    /// Сбрасывает настройки к значениям по умолчанию.
    /// </summary>
    public static void Reset()
    {
        _settings = new SettingsData();
        LogInfo("Settings reset.");
    }

    /// <summary>
    /// Применяет все настройки к системе.
    /// </summary>
    public static void ApplyToSystem()
    {
        ApplyScreenResolution();
        ApplyFullScreen();
        ApplyQuality();
        ApplyFrameRate();
        ApplyVSync();
        ApplyFileLogging();
    }

    /// <summary>
    /// Применяет разрешение экрана.
    /// </summary>
    public static void ApplyScreenResolution()
    {
        string[] values = ScreenResolution.Split('x');

        if (values.Length != 2)
        {
            LogWarning($"Invalid resolution format: {ScreenResolution}");
            return;
        }

        if (int.TryParse(values[0], out int width) &&
            int.TryParse(values[1], out int height))
        {
            Screen.SetResolution(width, height, IsFullScreen);
        }
    }

    /// <summary>
    /// Применяет полноэкранный режим.
    /// </summary>
    public static void ApplyFullScreen()
    {
        Screen.fullScreen = IsFullScreen;
    }

    /// <summary>
    /// Применяет громкость музыки к источнику.
    /// </summary>
    public static void ApplyMusicVolume(AudioSource source)
    {
        if (source == null)
        {
            return;
        }

        source.volume = MusicVolume / 100f;
    }

    /// <summary>
    /// Применяет громкость звуков к источнику.
    /// </summary>
    public static void ApplySoundVolume(AudioSource source)
    {
        if (source == null)
        {
            return;
        }

        source.volume = SoundVolume / 100f;
    }

    /// <summary>
    /// Применяет качество графики.
    /// </summary>
    public static void ApplyQuality()
    {
        if (QualitySettings.names.Length == 0)
        {
            return;
        }

        QualitySettings.SetQualityLevel(QualityLevel, true);
    }

    /// <summary>
    /// Применяет ограничение FPS.
    /// </summary>
    public static void ApplyFrameRate()
    {
        Application.targetFrameRate = FrameRate;
    }

    /// <summary>
    /// Применяет VSync.
    /// </summary>
    public static void ApplyVSync()
    {
        QualitySettings.vSyncCount = IsVSync ? 1 : 0;
    }

    /// <summary>
    /// Применяет настройку записи логов в файл.
    /// </summary>
    public static void ApplyFileLogging()
    {
        Logger.ConfigureFileLogging(IsFileLogging);
    }

    /// <summary>
    /// Загружает локализацию и применяет её к переданным UI-элементам.
    /// </summary>
    /// <param name="words">Словарь ключей и текстовых компонентов.</param>
    public static void ApplyLocalization(Dictionary<string, TMP_Text> words)
    {
        LocalizationService.LoadLocalization(LanguageCode);

        if (words == null)
        {
            return;
        }

        foreach (KeyValuePair<string, TMP_Text> pair in words)
        {
            if (pair.Value != null)
            {
                pair.Value.text = LocalizationService.GetTranslation(pair.Key);
            }
        }
    }

    /// <summary>
    /// Логирует информационное сообщение.
    /// </summary>
    private static void LogInfo(string message)
    {
        Logger.Log(LogLevel.Info, nameof(SettingsService), message);
    }

    /// <summary>
    /// Логирует предупреждение.
    /// </summary>
    private static void LogWarning(string message)
    {
        Logger.Log(LogLevel.Warning, nameof(SettingsService), message);
    }

    /// <summary>
    /// Логирует ошибку.
    /// </summary>
    private static void LogError(string message)
    {
        Logger.Log(LogLevel.Error, nameof(SettingsService), message);
    }
}