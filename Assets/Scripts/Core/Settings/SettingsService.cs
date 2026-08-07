using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Сервис управления настройками игры.
/// Отвечает за загрузку, сохранение, удаление и применение настроек игры.
/// </summary>
public static class SettingsService
{
    /// <summary>
    /// Репозиторий для работы с файлом настроек.
    /// </summary>
    private static readonly SettingsFileRepository _settingsFileRepository =
        new SettingsFileRepository(Globals.GameSettingsFilePath);

    /// <summary>
    /// Настройки, хранящиеся в памяти.
    /// </summary>
    private static SettingsData _settingsData = new SettingsData();



    /// <summary>
    /// Свойство получает или устанавливает разрешение экрана.
    /// Формат: "ШиринаxВысота".
    /// </summary>
    public static string ScreenResolution
    {
        get => _settingsData.ScreenResolution;
        set => _settingsData.ScreenResolution = value?.Trim() ?? _settingsData.ScreenResolution;
    }

    /// <summary>
    /// Свойство получает или устанавливает полноэкранный режим.
    /// </summary>
    public static bool IsFullScreen
    {
        get => _settingsData.IsFullScreen;
        set => _settingsData.IsFullScreen = value;
    }

    /// <summary>
    /// Свойство получает или устанавливает громкость музыки.
    /// Значение хранится в диапазоне от 0 до 100.
    /// </summary>
    public static float MusicVolume
    {
        get => _settingsData.MusicVolume;
        set => _settingsData.MusicVolume = Mathf.Clamp(value, 0f, 100f);
    }

    /// <summary>
    /// Свойство получает или устанавливает громкость звуков.
    /// Значение хранится в диапазоне от 0 до 100.
    /// </summary>
    public static float SoundVolume
    {
        get => _settingsData.SoundVolume;
        set => _settingsData.SoundVolume = Mathf.Clamp(value, 0f, 100f);
    }

    /// <summary>
    /// Свойство получает или устанавливает название локализации.
    /// </summary>
    public static string Localization
    {
        get => _settingsData.Localization;
        set => _settingsData.Localization = value?.Trim() ?? _settingsData.Localization;
    }

    /// <summary>
    /// Свойство получает или устанавливает код языка.
    /// </summary>
    public static string LanguageCode
    {
        get => _settingsData.LanguageCode;
        set => _settingsData.LanguageCode = value?.Trim() ?? _settingsData.LanguageCode;
    }

    /// <summary>
    /// Свойство получает или устанавливает включение записи логов в файл.
    /// </summary>
    public static bool IsFileLogging
    {
        get => _settingsData.IsFileLogging;
        set => _settingsData.IsFileLogging = value;
    }



    /// <summary>
    /// Метод сохраняет текущие настройки в файл.
    /// </summary>
    public static void Save()
    {
        try
        {
            SettingsDataValidator.Normalize(_settingsData);

            string json = JsonUtility.ToJson(_settingsData, true);

            if (string.IsNullOrWhiteSpace(json))
            {
                LogWarning("Settings serialization returned empty JSON. Save skipped.");
                return;
            }

            _settingsFileRepository.Save(json);

            LogInfo("Settings saved successfully.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to save settings: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод загружает настройки из файла.
    /// Если файл отсутствует, пустой или повреждён, загружает значения по умолчанию.
    /// <returns>True, если настройки загружены успешно; иначе false.</returns>
    /// </summary>
    public static bool Load()
    {
        try
        {
            if (!_settingsFileRepository.Exists())
            {
                LogWarning("Settings file not found. Loading default settings.");

                Default();

                return false;
            }

            string json = _settingsFileRepository.Load();

            if (string.IsNullOrWhiteSpace(json))
            {
                LogWarning("Settings file is empty. Loading default settings.");

                Default();

                return false;
            }

            SettingsData loaded = JsonUtility.FromJson<SettingsData>(json);

            if (loaded == null)
            {
                LogWarning("Failed to deserialize settings. Loading default settings.");

                Default();

                return false;
            }

            _settingsData = loaded;

            SettingsDataValidator.Normalize(_settingsData);

            LogInfo("Settings loaded successfully.");

            return true;
        }
        catch (Exception exception)
        {
            LogError($"Failed to load settings: {exception.Message}");

            Default();

            return false;
        }
    }

    /// <summary>
    /// Метод сбрасывает настройки к значениям по умолчанию.
    /// </summary>
    public static void Default()
    {
        _settingsData = new SettingsData();

        LogInfo("Settings reset to default values.");
    }



    /// <summary>
    /// Метод применяет текущие настройки из памяти.
    /// </summary>
    public static void Apply()
    {
        LogInfo("Applying current settings.");

        ApplyScreenResolution(_settingsData.ScreenResolution, _settingsData.IsFullScreen);
        ApplyFullScreen(_settingsData.IsFullScreen);
        ApplyFileLogging(_settingsData.IsFileLogging);

        LogInfo("Current settings applied successfully.");
    }

    /// <summary>
    /// Метод применяет разрешение экрана.
    /// </summary>
    /// <param name="screenResolution">Разрешение экрана в формате "ШиринаxВысота".</param>
    /// <param name="isFullScreen">Признак полноэкранного режима.</param>
    public static void ApplyScreenResolution(string screenResolution, bool isFullScreen)
    {
        if (string.IsNullOrWhiteSpace(screenResolution))
        {
            LogWarning("Screen resolution is empty. Apply skipped.");
            return;
        }

        string[] values = screenResolution.Split('x');

        if (values.Length != 2)
        {
            LogWarning($"Invalid resolution format: {screenResolution}");
            return;
        }

        if (!int.TryParse(values[0], out int width) || !int.TryParse(values[1], out int height))
        {
            LogWarning($"Failed to parse resolution values: {screenResolution}");
            return;
        }

        Screen.SetResolution(width, height, isFullScreen);

        LogInfo($"Screen resolution applied: {width}x{height}, fullscreen: {isFullScreen}");
    }

    /// <summary>
    /// Метод применяет полноэкранный режим.
    /// </summary>
    /// <param name="isFullScreen">True, если нужно включить полноэкранный режим; иначе false.</param>
    public static void ApplyFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

        LogInfo($"Fullscreen mode applied: {isFullScreen}");
    }

    /// <summary>
    /// Метод применяет громкость музыки к источнику.
    /// </summary>
    /// <param name="source">Источник музыки, к которому будет применена громкость.</param>
    /// <param name="volume">Громкость в диапазоне от 0 до 100.</param>
    public static void ApplyMusicVolume(AudioSource source, float volume)
    {
        if (source == null)
        {
            LogWarning("Music AudioSource is null. Apply skipped.");
            return;
        }

        source.volume = Mathf.Clamp(volume, 0f, 100f) / 100f;

        LogInfo($"Music volume applied: {volume}%");
    }

    /// <summary>
    /// Метод применяет громкость звуков к источнику.
    /// </summary>
    /// <param name="source">Источник звука, к которому будет применена громкость.</param>
    /// <param name="volume">Громкость в диапазоне от 0 до 100.</param>
    public static void ApplySoundVolume(AudioSource source, float volume)
    {
        if (source == null)
        {
            LogWarning("Sound AudioSource is null. Apply skipped.");
            return;
        }

        source.volume = Mathf.Clamp(volume, 0f, 100f) / 100f;

        LogInfo($"Sound volume applied: {volume}%");
    }

    /// <summary>
    /// Метод применяет настройку записи логов в файл.
    /// </summary>
    /// <param name="isFileLogging">True, если запись логов в файл должна быть включена; иначе false.</param>
    public static void ApplyFileLogging(bool isFileLogging)
    {
        Logger.ToggleFileLogging(isFileLogging);

        LogInfo($"File logging applied: {isFileLogging}");
    }

    /// <summary>
    /// Метод применяет локализацию к переданным UI-текстам.
    /// </summary>
    /// <param name="languageCode">Код языка.</param>
    /// <param name="localizationTargets">Словарь ключей текстовых компонентов и UI-текстовых компонентов.</param>
    public static void ApplyLocalization(string languageCode, Dictionary<string, TMP_Text> localizationTargets)
    {
        if (!LocalizationService.LoadLocalization(languageCode))
        {
            LogWarning($"Localization load failed for language code: {languageCode}");
            return;
        }

        if (localizationTargets == null)
        {
            LogWarning("Localization target dictionary is null. Apply skipped.");
            return;
        }

        foreach (KeyValuePair<string, TMP_Text> localizationTarget in localizationTargets)
        {
            if (localizationTarget.Value == null)
            {
                LogWarning($"TMP_Text is null for localization key: {localizationTarget.Key}");
                continue;
            }

            localizationTarget.Value.text = LocalizationService.GetTranslation(localizationTarget.Key);
        }

        LogInfo($"Localization applied successfully for language code: {languageCode}");
    }



    /// <summary>
    /// Метод записывает информационное сообщение в лог.
    /// </summary>
    private static void LogInfo(string message)
    {
        Logger.Log(LogLevel.Info, nameof(SettingsService), message);
    }

    /// <summary>
    /// Метод записывает предупреждение в лог.
    /// </summary>
    private static void LogWarning(string message)
    {
        Logger.Log(LogLevel.Warning, nameof(SettingsService), message);
    }

    /// <summary>
    /// Метод записывает ошибку в лог.
    /// </summary>
    private static void LogError(string message)
    {
        Logger.Log(LogLevel.Error, nameof(SettingsService), message);
    }
}