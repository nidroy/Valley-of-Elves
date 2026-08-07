using System.Collections.Generic;
using TMPro;
using UnityEditor.Localization.Editor;
using UnityEngine;

/// <summary>
/// Контроллер меню настроек.
/// Отвечает за логику взаимодействия с UI.
/// </summary>
public class SettingsMenuController
{
    #region Поля

    /// <summary>
    /// Словарь локализуемых UI-элементов: ключ текстового компонента -> текстовый компонент.
    /// </summary>
    private readonly Dictionary<string, TMP_Text> _localizationTargetsDictionary = new Dictionary<string, TMP_Text>();

    /// <summary>
    /// Временные данные настроек для UI.
    /// </summary>
    private SettingsData _tempSettingsData = new SettingsData();

    /// <summary>
    /// Флаг, отвечающий за изменение настроек.
    /// </summary>
    private bool _isSettingsChanged;

    #endregion



    #region Свойства

    /// <summary>
    /// Временные данные настроек для UI.
    /// </summary>
    public SettingsData TempSettingsData
    {
        get => _tempSettingsData;
        private set => _tempSettingsData = value;
    }

    /// <summary>
    /// Словарь локализуемых UI-элементов.
    /// </summary>
    public Dictionary<string, TMP_Text> LocalizationTargetsDictionary
    {
        get => _localizationTargetsDictionary;
    }

    /// <summary>
    /// Показывает, были ли изменены настройки.
    /// </summary>
    public bool IsSettingsChanged
    {
        get => _isSettingsChanged;
        private set => _isSettingsChanged = value;
    }

    #endregion



    /// <summary>
    /// Метод сбрасывает флаг изменения настроек.
    /// </summary>
    public void ResetSettingsChanged()
    {
        if (!IsSettingsChanged)
        {
            LogInfo("Settings changed flag is already reset.");
            return;
        }

        IsSettingsChanged = false;
        LogInfo("Settings changed flag reset.");
    }



    /// <summary>
    /// Метод восстанавливает временные настройки из настроек, хранящихся в памяти.
    /// </summary>
    public void RestoreSettings()
    {
        TempSettingsData = new SettingsData
        {
            ScreenResolution = SettingsService.ScreenResolution,
            IsFullScreen = SettingsService.IsFullScreen,
            MusicVolume = SettingsService.MusicVolume,
            SoundVolume = SettingsService.SoundVolume,
            Localization = SettingsService.Localization,
            LanguageCode = SettingsService.LanguageCode,
            IsFileLogging = SettingsService.IsFileLogging
        };

        LogInfo("Temporary settings restored from in-memory settings.");
    }



    #region Инициализация

    /// <summary>
    /// Основная инициализация меню настроек.
    /// </summary>
    /// <param name="localizationTargets">Список локализуемых UI-элементов.</param>
    /// <param name="musicSource">Источник музыки.</param>
    /// <param name="soundSources">Источники звуков.</param>
    public void InitializeSettings(List<LocalizationTarget> localizationTargets, AudioSource musicSource, AudioSource[] soundSources)
    {
        RestoreSettings();
        InitializeLocalizationTargets(localizationTargets);

        ApplyScreenResolution();
        ApplyFullScreen();
        ApplyMusicVolume(musicSource);
        ApplySoundVolume(soundSources);
        ApplyLocalization();
        ApplyFileLogging();

        IsSettingsChanged = false;

        LogInfo("Settings initialized.");
    }

    /// <summary>
    /// Метод собирает словарь локализуемых UI-элементов.
    /// </summary>
    private void InitializeLocalizationTargets(List<LocalizationTarget> localizationTargets)
    {
        _localizationTargetsDictionary.Clear();

        if (localizationTargets == null)
        {
            LogWarning("Localization targets list is null.");
            return;
        }

        for (int i = 0; i < localizationTargets.Count; i++)
        {
            LocalizationTarget target = localizationTargets[i];

            if (string.IsNullOrWhiteSpace(target.Key))
            {
                LogWarning($"Empty localization key at element #{i}.");
                continue;
            }

            if (target.Text == null)
            {
                LogWarning($"TMP_Text is not assigned for localization key: {target.Key}.");
                continue;
            }

            if (_localizationTargetsDictionary.ContainsKey(target.Key))
            {
                LogWarning($"Duplicate localization key: {target.Key}.");
                continue;
            }

            _localizationTargetsDictionary.Add(target.Key, target.Text);
        }

        LogInfo($"Localization targets dictionary built. Count: {_localizationTargetsDictionary.Count}.");
    }

    #endregion



    #region Применение настроек

    /// <summary>
    /// Метод применяет временные настройки к текущим настройкам, хранящимся в памяти.
    /// </summary>
    public void ApplySettings()
    {
        SettingsService.ScreenResolution = TempSettingsData.ScreenResolution;
        SettingsService.IsFullScreen = TempSettingsData.IsFullScreen;
        SettingsService.MusicVolume = TempSettingsData.MusicVolume;
        SettingsService.SoundVolume = TempSettingsData.SoundVolume;
        SettingsService.Localization = TempSettingsData.Localization;
        SettingsService.LanguageCode = TempSettingsData.LanguageCode;
        SettingsService.IsFileLogging = TempSettingsData.IsFileLogging;

        LogInfo("Temporary settings applied to in-memory settings.");
    }

    /// <summary>
    /// Метод применяет разрешение экрана.
    /// </summary>
    public void ApplyScreenResolution()
    {
        if (string.IsNullOrWhiteSpace(TempSettingsData.ScreenResolution))
        {
            LogWarning("Screen resolution is empty.");
            return;
        }

        SettingsService.ApplyScreenResolution(TempSettingsData.ScreenResolution, TempSettingsData.IsFullScreen);

        IsSettingsChanged = true;

        LogInfo($"Screen resolution applied: {TempSettingsData.ScreenResolution}, fullscreen: {TempSettingsData.IsFullScreen}.");
    }

    /// <summary>
    /// Метод применяет полноэкранный режим.
    /// </summary>
    public void ApplyFullScreen()
    {
        SettingsService.ApplyFullScreen(TempSettingsData.IsFullScreen);

        IsSettingsChanged = true;

        LogInfo($"Fullscreen mode applied: {TempSettingsData.IsFullScreen}.");
    }

    /// <summary>
    /// Метод применяет громкость музыки.
    /// </summary>
    /// <param name="musicSource">Источник музыки.</param>
    public void ApplyMusicVolume(AudioSource musicSource)
    {
        SettingsService.ApplyMusicVolume(musicSource, TempSettingsData.MusicVolume);

        IsSettingsChanged = true;

        LogInfo($"Music volume applied: {TempSettingsData.MusicVolume:0.00}.");
    }

    /// <summary>
    /// Метод применяет громкость звуков.
    /// </summary>
    /// <param name="soundSources">Массив источников звуков.</param>
    public void ApplySoundVolume(AudioSource[] soundSources)
    {
        if (soundSources == null || soundSources.Length == 0)
        {
            LogWarning("Sound sources array is empty.");
            return;
        }

        for (int i = 0; i < soundSources.Length; i++)
        {
            AudioSource soundSource = soundSources[i];

            if (soundSource == null)
            {
                LogWarning($"Sound source at index {i} is null.");
                continue;
            }

            SettingsService.ApplySoundVolume(soundSource, TempSettingsData.SoundVolume);
        }

        IsSettingsChanged = true;

        LogInfo($"Sound volume applied: {TempSettingsData.SoundVolume:0.00}.");
    }

    /// <summary>
    /// Метод применяет локализацию.
    /// </summary>
    public void ApplyLocalization()
    {
        if (string.IsNullOrWhiteSpace(TempSettingsData.LanguageCode))
        {
            LogWarning("Language code is empty.");
            return;
        }

        SettingsService.ApplyLocalization(TempSettingsData.LanguageCode, _localizationTargetsDictionary);

        IsSettingsChanged = true;

        LogInfo($"Localization applied: {TempSettingsData.LanguageCode}.");
    }

    /// <summary>
    /// Метод применяет запись логов в файл.
    /// </summary>
    public void ApplyFileLogging()
    {
        SettingsService.ApplyFileLogging(TempSettingsData.IsFileLogging);

        IsSettingsChanged = true;

        LogInfo($"File logging applied: {TempSettingsData.IsFileLogging}.");
    }

    #endregion



    #region Логирование

    /// <summary>
    /// Метод записывает информационное сообщение в лог.
    /// </summary>
    private static void LogInfo(string message)
    {
        Logger.Log(LogLevel.Info, nameof(SettingsMenuController), message);
    }

    /// <summary>
    /// Метод записывает предупреждение в лог.
    /// </summary>
    private static void LogWarning(string message)
    {
        Logger.Log(LogLevel.Warning, nameof(SettingsMenuController), message);
    }

    /// <summary>
    /// Метод записывает ошибку в лог.
    /// </summary>
    private static void LogError(string message)
    {
        Logger.Log(LogLevel.Error, nameof(SettingsMenuController), message);
    }

    #endregion
}



#region Структуры

/// <summary>
/// Структура для варианта разрешения экрана.
/// </summary>
[System.Serializable]
public struct ScreenResolutionOption
{
    /// <summary>
    /// Ширина экрана.
    /// </summary>
    public int Width;

    /// <summary>
    /// Высота экрана.
    /// </summary>
    public int Height;

    /// <summary>
    /// Создаёт новый вариант разрешения экрана.
    /// </summary>
    /// <param name="width">Ширина.</param>
    /// <param name="height">Высота.</param>
    public ScreenResolutionOption(int width, int height)
    {
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Отображаемая строка разрешения экрана.
    /// </summary>
    public string DisplayName => $"{Width}x{Height}";
}

/// <summary>
/// Структура для варианта локализации.
/// </summary>
[System.Serializable]
public struct LocalizationOption
{
    /// <summary>
    /// Отображаемое имя локализации.
    /// </summary>
    public string Name;

    /// <summary>
    /// Код локализации.
    /// </summary>
    public string Code;

    /// <summary>
    /// Создаёт новый вариант локализации.
    /// </summary>
    /// <param name="name">Имя.</param>
    /// <param name="code">Код.</param>
    public LocalizationOption(string name, string code)
    {
        Name = name;
        Code = code;
    }
}

/// <summary>
/// Структура для привязки локализуемого текстового компонента к ключу текстового компонента.
/// </summary>
[System.Serializable]
public struct LocalizationTarget
{
    /// <summary>
    /// Ключ текстового компонента.
    /// </summary>
    public string Key;

    /// <summary>
    /// Текстовый компонент, который нужно локализовать.
    /// </summary>
    public TMP_Text Text;

    /// <summary>
    /// Создаёт новую привязку локализации.
    /// </summary>
    /// <param name="key">Ключ текстового компонента.</param>
    /// <param name="text">Текстовый компонент.</param>
    public LocalizationTarget(string key, TMP_Text text)
    {
        Key = key;
        Text = text;
    }
}

#endregion