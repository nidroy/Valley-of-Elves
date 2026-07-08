using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс управляет меню настроек игры.
/// </summary>
public class SettingsMenuManager : MonoBehaviour
{
    #region Приватные поля

    // Выпадающий список разрешения экрана.
    [SerializeField]
    private TMP_Dropdown _screenResolutionDropdown;

    // Выпадающий список локализации.
    [SerializeField]
    private TMP_Dropdown _localizationDropdown;

    // Переключатель полноэкранного режима.
    [SerializeField]
    private Toggle _fullScreenToggle;

    // Переключатель записи логов в файл.
    [SerializeField]
    private Toggle _fileLoggingToggle;

    // Ползунок громкости музыки.
    [SerializeField]
    private Slider _musicVolumeSlider;

    // Ползунок громкости звуков.
    [SerializeField]
    private Slider _soundVolumeSlider;

    // Текст значения громкости музыки.
    [SerializeField]
    private TMP_Text _musicVolumeText;

    // Текст значения громкости звуков.
    [SerializeField]
    private TMP_Text _soundVolumeText;

    // Источник музыки.
    [SerializeField]
    private AudioSource _musicSource;

    // Источники игровых звуков.
    [SerializeField]
    private AudioSource[] _soundSources;

    // Элементы интерфейса для локализации.
    [SerializeField]
    private List<Word> _words;

    #endregion


    #region Приватные классы

    /// <summary>
    /// Класс хранит элемент интерфейса и его ключ локализации.
    /// </summary>
    [Serializable]
    private class Word
    {
        // Ключ локализации.
        public string KeyWord;

        // Текстовый компонент интерфейса.
        public TMP_Text WordText;
    }

    /// <summary>
    /// Структура хранит данные разрешения экрана.
    /// </summary>
    private struct ScreenResolution
    {
        // Ширина экрана.
        public int Width;

        // Высота экрана.
        public int Height;

        // Текстовое представление разрешения.
        public string DisplayString => $"{Width}x{Height}";

        /// <summary>
        /// Структура создает объект разрешения экрана.
        /// </summary>
        /// <param name="width">Ширина экрана.</param>
        /// <param name="height">Высота экрана.</param>
        public ScreenResolution(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    /// <summary>
    /// Структура хранит данные локализации.
    /// </summary>
    private struct Localization
    {
        // Название языка.
        public string Language;

        // Код языка.
        public string LanguageCode;

        /// <summary>
        /// Структура создает объект локализации.
        /// </summary>
        /// <param name="language">Название языка.</param>
        /// <param name="languageCode">Код языка.</param>
        public Localization(string language, string languageCode)
        {
            Language = language;
            LanguageCode = languageCode;
        }
    }

    #endregion


    #region Данные настроек

    // Доступные разрешения экрана.
    private readonly ScreenResolution[] _screenResolutions =
    {
        new ScreenResolution(1920, 1080),
        new ScreenResolution(1600, 900),
        new ScreenResolution(1366, 768),
        new ScreenResolution(1280, 720),
        new ScreenResolution(2560, 1440),
        new ScreenResolution(3840, 2160),
        new ScreenResolution(640, 360)
    };

    // Доступные локализации игры.
    private readonly Localization[] _localizations =
    {
        new Localization("English", "EN"),
        new Localization("Русский", "RU")
    };

    #endregion


    #region Публичные методы

    /// <summary>
    /// Метод инициализирует систему настроек.
    /// </summary>
    public void Init()
    {
        try
        {
            // Загружаем сохраненные настройки.
            LoadSettings();

            // Применяем настройки к игре.
            ApplySettings();

            LogInfo("Settings system initialized.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to initialize settings system: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод сохраняет текущие настройки.
    /// </summary>
    public void ApplySettingsButtonClick()
    {
        try
        {
            // Сохраняем настройки.
            SaveSettings();

            LogInfo("Settings saved.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to save settings: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод отменяет текущие изменения настроек.
    /// </summary>
    public void CancelSettingsButtonClick()
    {
        try
        {
            // Загружаем сохраненные настройки.
            LoadSettings();

            // Применяем сохраненные значения.
            ApplySettings();

            // Обновляем элементы интерфейса.
            UpdateSettingsUI();

            LogInfo("Settings changes canceled.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to cancel settings changes: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод подготавливает интерфейс меню настроек.
    /// </summary>
    public void PrepareSettingsMenu()
    {
        try
        {
            // Загружаем текущие настройки.
            LoadSettings();

            // Обновляем значения элементов интерфейса.
            UpdateSettingsUI();

            LogInfo("Settings menu prepared.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to prepare settings menu: {exception.Message}");
        }
    }

    #endregion


    #region Обработчики интерфейса

    /// <summary>
    /// Обработчик изменяет состояние полноэкранного режима.
    /// </summary>
    public void OnFullScreenToggleChanged()
    {
        try
        {
            // Проверяем наличие переключателя.
            if (_fullScreenToggle == null)
            {
                LogError("Fullscreen toggle is null.");
                return;
            }


            // Сохраняем новое значение.
            Settings.IsFullScreen = _fullScreenToggle.isOn;


            // Применяем режим экрана.
            Settings.ApplyFullScreen();


            LogInfo($"Fullscreen mode changed: {Settings.IsFullScreen}.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to change fullscreen mode: {exception.Message}");
        }
    }

    /// <summary>
    /// Обработчик изменяет состояние записи логов в файл.
    /// </summary>
    public void OnFileLoggingToggleChanged()
    {
        try
        {
            // Проверяем наличие переключателя.
            if (_fileLoggingToggle == null)
            {
                LogError("File logging toggle is null.");
                return;
            }

            // Сохраняем новое значение.
            Settings.IsFileLogging = _fileLoggingToggle.isOn;

            // Применяем настройку логирования.
            Settings.ApplyFileLogging();

            LogInfo($"File logging changed: {Settings.IsFileLogging}.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to change file logging: {exception.Message}");
        }
    }

    /// <summary>
    /// Обработчик изменяет громкость музыки.
    /// </summary>
    public void OnMusicVolumeSliderChanged()
    {
        try
        {
            // Проверяем наличие слайдера.
            if (_musicVolumeSlider == null)
            {
                LogError("Music volume slider is null.");
                return;
            }

            // Сохраняем громкость.
            Settings.MusicVolume = _musicVolumeSlider.value;

            // Применяем громкость.
            Settings.ApplyMusicVolume(_musicSource);

            // Обновляем отображение значения.
            UpdateMusicVolumeText();

            LogInfo($"Music volume changed: {Settings.MusicVolume}%.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to change music volume: {exception.Message}");
        }
    }

    /// <summary>
    /// Обработчик изменяет громкость звуков.
    /// </summary>
    public void OnSoundVolumeSliderChanged()
    {
        try
        {
            // Проверяем наличие слайдера.
            if (_soundVolumeSlider == null)
            {
                LogError("Sound volume slider is null.");
                return;
            }

            // Сохраняем громкость.
            Settings.SoundVolume = _soundVolumeSlider.value;

            // Применяем громкость ко всем источникам.
            ApplySoundVolumeToSources();

            // Обновляем отображение значения.
            UpdateSoundVolumeText();

            LogInfo($"Sound volume changed: {Settings.SoundVolume}%.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to change sound volume: {exception.Message}");
        }
    }

    /// <summary>
    /// Обработчик изменяет разрешение экрана.
    /// </summary>
    /// <param name="index">Индекс выбранного разрешения.</param>
    public void OnScreenResolutionChanged(int index)
    {
        try
        {
            // Проверяем индекс.
            if (!ValidateResolutionIndex(index))
            {
                LogError("Invalid screen resolution index.");
                return;
            }

            // Сохраняем разрешение.
            Settings.ScreenResolution = _screenResolutions[index].DisplayString;

            // Применяем разрешение.
            Settings.ApplyScreenResolution();

            LogInfo($"Screen resolution changed: {Settings.ScreenResolution}.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to change screen resolution: {exception.Message}");
        }
    }

    /// <summary>
    /// Обработчик изменяет локализацию игры.
    /// </summary>
    /// <param name="index">Индекс выбранной локализации.</param>
    public void OnLocalizationChanged(int index)
    {
        try
        {
            // Проверяем индекс.
            if (!ValidateLocalizationIndex(index))
            {
                LogError("Invalid localization index.");
                return;
            }

            // Сохраняем язык.
            Settings.Localization = _localizations[index].Language;

            // Сохраняем код языка.
            Settings.LanguageCode = _localizations[index].LanguageCode;

            // Применяем локализацию.
            ApplyLocalization();

            LogInfo($"Localization changed: {Settings.Localization}.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to change localization: {exception.Message}");
        }
    }

    #endregion


    #region Применение настроек

    /// <summary>
    /// Метод загружает настройки из файла.
    /// </summary>
    private void LoadSettings()
    {
        // Загружаем настройки.
        Settings.Load();

        LogInfo("Settings loaded.");
    }

    /// <summary>
    /// Метод сохраняет настройки в файл.
    /// </summary>
    private void SaveSettings()
    {
        // Сохраняем настройки.
        Settings.Save();

        LogInfo("Settings saved.");
    }

    /// <summary>
    /// Метод применяет текущие настройки игры.
    /// </summary>
    private void ApplySettings()
    {
        try
        {
            // Применяем разрешение экрана.
            Settings.ApplyScreenResolution();

            // Применяем локализацию.
            ApplyLocalization();

            // Применяем полноэкранный режим.
            Settings.ApplyFullScreen();

            // Применяем громкость музыки.
            Settings.ApplyMusicVolume(_musicSource);

            // Применяем громкость звуков.
            ApplySoundVolumeToSources();

            // Применяем состояние логирования.
            Settings.ApplyFileLogging();

            LogInfo("Settings applied.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to apply settings: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод применяет локализацию элементов интерфейса.
    /// </summary>
    private void ApplyLocalization()
    {
        // Получаем словарь элементов интерфейса.
        Dictionary<string, TMP_Text> words = CreateLocalizationDictionary();

        // Применяем локализацию.
        Settings.ApplyLocalization(words);
    }

    /// <summary>
    /// Метод создает словарь элементов интерфейса.
    /// </summary>
    /// <returns>Словарь ключей и текстовых компонентов.</returns>
    private Dictionary<string, TMP_Text> CreateLocalizationDictionary()
    {
        // Создаем новый словарь.
        Dictionary<string, TMP_Text> words = new Dictionary<string, TMP_Text>();

        // Проверяем наличие списка элементов.
        if (_words == null)
        {
            LogWarning("Localization words list is null.");

            return words;
        }

        // Добавляем элементы в словарь.
        foreach (Word word in _words)
        {
            // Проверяем корректность элемента.
            if (word == null || word.WordText == null)
            {
                LogWarning("Invalid localization element skipped.");

                continue;
            }

            // Добавляем перевод.
            words[word.KeyWord] = word.WordText;
        }

        return words;
    }

    #endregion


    #region Обновление интерфейса

    /// <summary>
    /// Метод обновляет все элементы меню настроек.
    /// </summary>
    private void UpdateSettingsUI()
    {
        // Обновляем разрешение экрана.
        UpdateScreenResolutionDropdown();

        // Обновляем локализацию.
        UpdateLocalizationDropdown();

        // Обновляем полноэкранный режим.
        UpdateFullScreenToggle();

        // Обновляем логирование.
        UpdateFileLoggingToggle();

        // Обновляем громкость музыки.
        UpdateMusicVolumeSlider();

        // Обновляем громкость звуков.
        UpdateSoundVolumeSlider();
    }

    /// <summary>
    /// Метод загружает разрешения экрана в список.
    /// </summary>
    private void UpdateScreenResolutionDropdown()
    {
        // Проверяем наличие элемента.
        if (_screenResolutionDropdown == null)
        {
            LogError("Screen resolution dropdown is null.");

            return;
        }

        // Очищаем старые значения.
        _screenResolutionDropdown.ClearOptions();

        // Создаем список разрешений.
        List<string> options = new List<string>();

        // Добавляем доступные разрешения.
        foreach (ScreenResolution resolution in _screenResolutions)
        {
            options.Add(resolution.DisplayString);
        }

        // Заполняем список.
        _screenResolutionDropdown.AddOptions(options);

        // Устанавливаем текущее значение.
        _screenResolutionDropdown.value = GetScreenResolutionIndex();

        // Обновляем отображение.
        _screenResolutionDropdown.RefreshShownValue();

        // Настраиваем обработчик.
        _screenResolutionDropdown.onValueChanged.RemoveAllListeners();

        _screenResolutionDropdown.onValueChanged.AddListener(OnScreenResolutionChanged);
    }

    /// <summary>
    /// Метод загружает локализации в список.
    /// </summary>
    private void UpdateLocalizationDropdown()
    {
        // Проверяем наличие элемента.
        if (_localizationDropdown == null)
        {
            LogError("Localization dropdown is null.");

            return;
        }

        // Очищаем старые значения.
        _localizationDropdown.ClearOptions();

        // Создаем список языков.
        List<string> options = new List<string>();

        // Добавляем доступные языки.
        foreach (Localization localization in _localizations)
        {
            options.Add(localization.Language);
        }

        // Заполняем список.
        _localizationDropdown.AddOptions(options);

        // Устанавливаем текущий язык.
        _localizationDropdown.value = GetLocalizationIndex();

        // Обновляем отображение.
        _localizationDropdown.RefreshShownValue();

        // Настраиваем обработчик.
        _localizationDropdown.onValueChanged.RemoveAllListeners();

        _localizationDropdown.onValueChanged.AddListener(OnLocalizationChanged);
    }

    /// <summary>
    /// Метод обновляет состояние полноэкранного режима.
    /// </summary>
    private void UpdateFullScreenToggle()
    {
        // Проверяем наличие элемента.
        if (_fullScreenToggle == null)
        {
            LogError("Fullscreen toggle is null.");

            return;
        }

        // Устанавливаем состояние переключателя.
        _fullScreenToggle.isOn = Settings.IsFullScreen;
    }

    /// <summary>
    /// Метод обновляет состояние записи логов.
    /// </summary>
    private void UpdateFileLoggingToggle()
    {
        // Проверяем наличие элемента.
        if (_fileLoggingToggle == null)
        {
            LogError("File logging toggle is null.");

            return;
        }

        // Устанавливаем состояние переключателя.
        _fileLoggingToggle.isOn = Settings.IsFileLogging;
    }

    /// <summary>
    /// Метод обновляет значение громкости музыки.
    /// </summary>
    private void UpdateMusicVolumeSlider()
    {
        // Проверяем наличие элемента.
        if (_musicVolumeSlider == null)
        {
            LogError("Music volume slider is null.");

            return;
        }

        // Устанавливаем значение громкости.
        _musicVolumeSlider.value = Settings.MusicVolume;

        // Обновляем текст.
        UpdateMusicVolumeText();
    }

    /// <summary>
    /// Метод обновляет значение громкости звуков.
    /// </summary>
    private void UpdateSoundVolumeSlider()
    {
        // Проверяем наличие элемента.
        if (_soundVolumeSlider == null)
        {
            LogError("Sound volume slider is null.");

            return;
        }

        // Устанавливаем значение громкости.
        _soundVolumeSlider.value = Settings.SoundVolume;

        // Обновляем текст.
        UpdateSoundVolumeText();
    }

    /// <summary>
    /// Метод обновляет текст громкости музыки.
    /// </summary>
    private void UpdateMusicVolumeText()
    {
        // Проверяем наличие текста.
        if (_musicVolumeText == null)
        {
            LogError("Music volume text is null.");

            return;
        }

        // Отображаем значение громкости.
        _musicVolumeText.text = Settings.MusicVolume.ToString("F0");
    }

    /// <summary>
    /// Метод обновляет текст громкости звуков.
    /// </summary>
    private void UpdateSoundVolumeText()
    {
        // Проверяем наличие текста.
        if (_soundVolumeText == null)
        {
            LogError("Sound volume text is null.");

            return;
        }

        // Отображаем значение громкости.
        _soundVolumeText.text = Settings.SoundVolume.ToString("F0");
    }

    #endregion


    #region Работа со звуком

    /// <summary>
    /// Метод применяет громкость ко всем источникам звуков.
    /// </summary>
    private void ApplySoundVolumeToSources()
    {
        // Проверяем наличие источников.
        if (_soundSources == null || _soundSources.Length == 0)
        {
            LogWarning("Sound sources collection is empty.");

            return;
        }

        // Применяем громкость каждому источнику.
        foreach (AudioSource source in _soundSources)
        {
            Settings.ApplySoundVolume(source);
        }
    }

    #endregion


    #region Получение индексов

    /// <summary>
    /// Метод получает индекс текущего разрешения экрана.
    /// </summary>
    /// <returns>Индекс разрешения.</returns>
    private int GetScreenResolutionIndex()
    {
        // Ищем совпадение разрешения.
        for (int index = 0; index < _screenResolutions.Length; index++)
        {
            if (_screenResolutions[index].DisplayString == Settings.ScreenResolution)
            {
                return index;
            }
        }

        // Возвращаем первое значение по умолчанию.
        return 0;
    }

    /// <summary>
    /// Метод получает индекс текущей локализации.
    /// </summary>
    /// <returns>Индекс локализации.</returns>
    private int GetLocalizationIndex()
    {
        // Ищем совпадение языка.
        for (int index = 0; index < _localizations.Length; index++)
        {
            if (_localizations[index].Language == Settings.Localization)
            {
                return index;
            }
        }

        // Возвращаем первое значение по умолчанию.
        return 0;
    }

    #endregion


    #region Проверки

    /// <summary>
    /// Метод проверяет индекс разрешения.
    /// </summary>
    /// <param name="index">Проверяемый индекс.</param>
    /// <returns>True, если индекс корректный.</returns>
    private bool ValidateResolutionIndex(int index)
    {
        // Проверяем границы массива.
        return index >= 0 &&
               index < _screenResolutions.Length;
    }

    /// <summary>
    /// Метод проверяет индекс локализации.
    /// </summary>
    /// <param name="index">Проверяемый индекс.</param>
    /// <returns>True, если индекс корректный.</returns>
    private bool ValidateLocalizationIndex(int index)
    {
        // Проверяем границы массива.
        return index >= 0 &&
               index < _localizations.Length;
    }

    #endregion


    #region Логирование

    /// <summary>
    /// Метод записывает информационное сообщение.
    /// </summary>
    /// <param name="message">Сообщение для записи.</param>
    private void LogInfo(string message)
    {
        Logger.Log(Logger.LogLevel.Info, nameof(SettingsMenuManager), message);
    }

    /// <summary>
    /// Метод записывает предупреждение.
    /// </summary>
    /// <param name="message">Сообщение предупреждения.</param>
    private void LogWarning(string message)
    {
        Logger.Log(Logger.LogLevel.Warning, nameof(SettingsMenuManager), message);
    }

    /// <summary>
    /// Метод записывает ошибку.
    /// </summary>
    /// <param name="message">Сообщение ошибки.</param>
    private void LogError(string message)
    {
        Logger.Log(Logger.LogLevel.Error, nameof(SettingsMenuManager), message);
    }

    #endregion
}