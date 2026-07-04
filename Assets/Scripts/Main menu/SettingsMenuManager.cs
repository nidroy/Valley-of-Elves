using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _settingsMenuObject; // Объект, отображающий меню настроек
    [SerializeField]
    private TMP_Dropdown _screenResolutionDropdown; // Поле для выбора разрешения экрана
    [SerializeField]
    private TMP_Dropdown _localizationDropdown; // Поле для выбора локализации игры
    [SerializeField]
    private Toggle _fullScreenToggle; // Чекбокс для переключения состояния полноэкранного режима
    [SerializeField]
    private Toggle _fileLoggingToggle; // Чекбокс для переключения состояния записи логов в файл
    [SerializeField]
    private Slider _musicVolumeSlider; // Слайдер для управления громкостью музыки
    [SerializeField]
    private Slider _soundVolumeSlider; // Слайдер для управления громкостью звуков
    [SerializeField]
    private TMP_Text _musicVolumeText; // Текстовое поле для отображения значения громкости музыки
    [SerializeField]
    private TMP_Text _soundVolumeText; // Текстовое поле для отображения значения громкости звуков
    [SerializeField]
    private AudioSource _musicSource; // Источник звука для музыки
    [SerializeField]
    private AudioSource[] _soundSource; // Источники звука для звуков
    [SerializeField]
    private List<Word> _words; // Текстовые поля для отображения слов в игре с ключевыми словами для них

    // <summary>
    /// Класс для хранения текстовых полей для отображения слов в игре с ключевыми словами для них
    /// </summary>
    [Serializable]
    private class Word
    {
        public string KeyWord;      // Ключевое слово
        public TMP_Text WordText;    // Текстовое поле для отображения слова
    }

    // Структура для хранения разрешения экрана
    private struct ScreenResolution
    {
        public int Width; // Ширина разрешения
        public int Height; // Высота разрешения
        public string DisplayString => $"{Width}x{Height}"; // Строковое представление разрешения

        public ScreenResolution(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    // Структура для хранения локализации игры
    private struct Localization
    {
        public string Language; // Язык локализации
        public string LanguageCode; // Код языка локализации

        public Localization(string language, string languageCode)
        {
            Language = language;
            LanguageCode = languageCode;
        }
    }

    // Массив доступных разрешений экрана формата 16:9
    private readonly ScreenResolution[] _screenResolutions = new ScreenResolution[]
    {
        new ScreenResolution(1920, 1080), // Full HD
        new ScreenResolution(1600, 900),  // HD+
        new ScreenResolution(1366, 768),  // HD+
        new ScreenResolution(1280, 720),  // HD
        new ScreenResolution(2560, 1440), // QHD
        new ScreenResolution(3840, 2160), // 4K
        new ScreenResolution(640, 360)    // SD
    };

    // Массив доступных локализаций игры
    private readonly Localization[] _localizations = new Localization[]
    {
        new Localization("English","EN"),
        new Localization("Русский","RU")
    };

    #region Публичные методы

    /// <summary>
    /// Метод для инициализации настроек
    /// </summary>
    public void Init()
    {
        LoadSettings(); // Загрузка настроек из файла
        ApplySettings(); // Применение текущих настроек
        LogInfo("Settings initialized successfully.");
    }

    /// <summary>
    /// Обработчик нажатия кнопки для открытия меню настроек и загрузки текущих настроек из файла
    /// </summary>
    public void OnOpenSettingsMenuButtonClick()
    {
        LoadSettings(); // Загрузка настроек из файла
        LoadScreeResolutionsToDropdown(); // Загрузка доступных разрешений экрана в выпадающий список
        LoadlocalizationsToDropdown(); // Загрузка доступных локализаций в выпадающий список
        UpdateFullScreenToggle(); // Обновление состояния чекбокса полноэкранного режим
        UpdateMusicVolumeSlider(); // Обновление значения слайдера громкости музыки
        UpdateSoundVolumeSlider(); // Обновление значения слайдера громкости звуков
        UpdateFileLoggingToggle(); // Обновление состояния чекбокса записи логов в файл
        ApplySettings(); // Применение текущих настроек
        _settingsMenuObject.SetActive(true); // Отображение меню настроек
        LogInfo("Settings menu opened.");
    }

    /// <summary>
    /// Обработчик нажатия кнопки для применения текущих настроек и закрытия меню
    /// </summary>
    public void OnApplySettingsButtonClick()
    {
        SaveSettings(); // Сохранение настроек в файл
        _settingsMenuObject.SetActive(false); // Закрытие меню настроек
        LogInfo("Settings applied and menu closed.");
    }

    /// <summary>
    /// Обработчик нажатия кнопки для отмены текущих настроек и закрытия меню
    /// </summary>
    public void OnCancelSettingsButtonClick()
    {
        LoadSettings(); // Загрузка настроек из файла
        ApplySettings(); // Применение текущих настроек
        _settingsMenuObject.SetActive(false); // Закрытие меню настроек
        LogInfo("Settings canceled and menu closed.");
    }

    /// <summary>
    /// Обработчик изменения состояния чекбокса для переключения состояния полноэкранного режима
    /// </summary>
    public void OnFullScreenToggleChanged()
    {
        Settings.IsFullScreen = _fullScreenToggle.isOn; // Сохраняем текущее состояние полноэкранного режима в настройках
        Settings.ApplyFullScreen(); // Применяем состояние полноэкранного режима
        LogInfo($"Full screen mode set to: {Settings.IsFullScreen}.");
    }

    /// <summary>
    /// Обработчик изменения громкости музыки слайдером
    /// </summary>
    public void OnMusicVolumeSliderChanged()
    {
        Settings.MusicVolume = _musicVolumeSlider.value; // Сохраняем текущее значение громкости в настройках
        Settings.ApplyMusicVolume(_musicSource); // Применяем выбранную громкость музыки
        UpdateMusicVolumeText(); // Обновляем текстовое поле громкости
        LogInfo($"Music volume set to: {Settings.MusicVolume}.");
    }

    /// <summary>
    /// Обработчик изменения громкости звуков слайдером
    /// </summary>
    public void OnSoundVolumeSliderChanged()
    {
        Settings.SoundVolume = _soundVolumeSlider.value; // Сохраняем текущее значение громкости в настройках
        if (_soundSource.Length > 0)
            foreach (AudioSource sound in _soundSource)
                Settings.ApplySoundVolume(sound); // Применяем выбранную громкость звуков
        UpdateSoundVolumeText(); // Обновляем текстовое поле громкости
        LogInfo($"Sound volume set to: {Settings.SoundVolume}.");
    }

    /// <summary>
    /// Обработчик изменения состояния чекбокса для переключения состояния записи логов в файл
    /// </summary>
    public void OnFileLoggingToggleChanged()
    {
        Settings.IsFileLogging = _fileLoggingToggle.isOn; // Сохраняем текущее состояние записи логов в настройках
        Settings.ApplyFileLogging(); // Применяем состояние записи логов
        LogInfo($"File logging set to: {Settings.IsFileLogging}.");
    }

    #endregion

    #region Приватные методы

    /// <summary>
    /// Метод для загрузки настроек из файла
    /// </summary>
    private void LoadSettings()
    {
        Settings.Load(); // Загрузка настроек
        LogInfo("Settings loaded successfully.");
    }

    /// <summary>
    /// Метод для сохранения текущих настроек в файл
    /// </summary>
    private void SaveSettings()
    {
        Settings.Save(); // Сохранение настроек
        LogInfo("Settings saved to file.");
    }

    /// <summary>
    /// Метод для применения текущих настроек
    /// </summary>
    private void ApplySettings()
    {
        Settings.ApplyScreenResolution(); // Применение разрешения экрана из настроек
        var words = new Dictionary<string, TMP_Text>();
        foreach (var word in _words)
            words.Add(word.KeyWord, word.WordText);
        Settings.ApplyLocalization(words); // Применение локализации из настроек
        Settings.ApplyFullScreen(); // Применение состояния полноэкранного режима
        Settings.ApplyMusicVolume(_musicSource); // Применение громкости музыки
        if (_soundSource.Length > 0)
            foreach (AudioSource sound in _soundSource)
                Settings.ApplySoundVolume(sound); // Применение громкости звуков
        Settings.ApplyFileLogging(); // Применение состояния записи логов в файл
        LogInfo("Settings applied successfully.");
    }

    /// <summary>
    /// Метод для загрузки доступных разрешений экрана в выпадающий список
    /// </summary>
    private void LoadScreeResolutionsToDropdown()
    {
        _screenResolutionDropdown.ClearOptions(); // Очистка текущих опций

        List<string> options = new List<string>();
        foreach (var res in _screenResolutions)
        {
            options.Add(res.DisplayString); // Добавление нового разрешения экрана
        }
        _screenResolutionDropdown.AddOptions(options); // Добавление новых опций

        int currentIndex = GetScreeResolutionIndex(Settings.ScreenResolution); // Получаем текущий индекс разрешения экрана
        _screenResolutionDropdown.value = currentIndex; // Устанавливаем выбранное значение
        _screenResolutionDropdown.RefreshShownValue(); // Обновление отображаемого значения

        // Удаление предыдущих обработчиков событий и добавление нового
        _screenResolutionDropdown.onValueChanged.RemoveAllListeners();
        _screenResolutionDropdown.onValueChanged.AddListener(OnChangeScreenResolution);
    }

    /// <summary>
    /// Метод для загрузки доступных локализаций в выпадающий список
    /// </summary>
    private void LoadlocalizationsToDropdown()
    {
        _localizationDropdown.ClearOptions(); // Очистка текущих опций

        List<string> options = new List<string>();
        foreach (var localization in _localizations)
        {
            options.Add(localization.Language); // Добавление новой локализации
        }
        _localizationDropdown.AddOptions(options); // Добавление новых опций

        int currentIndex = GetLocalizationIndex(Settings.Localization); // Получаем текущий индекс рлокализации
        _localizationDropdown.value = currentIndex; // Устанавливаем выбранное значение
        _localizationDropdown.RefreshShownValue(); // Обновление отображаемого значения

        // Удаление предыдущих обработчиков событий и добавление нового
        _localizationDropdown.onValueChanged.RemoveAllListeners();
        _localizationDropdown.onValueChanged.AddListener(OnChangeLocalization);
    }

    /// <summary>
    /// Обработчик изменения разрешения экрана из выпадающего списка
    /// </summary>
    /// <param name="index">Индекс выбранного разрешения экрана</param>
    private void OnChangeScreenResolution(int index)
    {
        Settings.ScreenResolution = _screenResolutions[index].DisplayString; // Сохраняем текущее разрешение экрана в настройках
        Settings.ApplyScreenResolution(); // Применяем выбранное разрешение экрана
        LogInfo($"Screen resolution changed to: {Settings.ScreenResolution}.");
    }

    /// <summary>
    /// Обработчик изменения локализации из выпадающего списка
    /// </summary>
    /// <param name="index">Индекс выбранной локализации</param>
    private void OnChangeLocalization(int index)
    {
        Settings.Localization = _localizations[index].Language; // Сохраняем текущую локализацию в настройках
        Settings.LanguageCode = _localizations[index].LanguageCode; // Сохраняем текущий код языка в настройках
        var words = new Dictionary<string, TMP_Text>();
        foreach (var word in _words)
            words.Add(word.KeyWord, word.WordText);
        Settings.ApplyLocalization(words); // Применяем выбранную локализацию
        LogInfo($"Localization changed to: {Settings.Localization}.");
    }

    /// <summary>
    /// Метод для получения индекса указанного разрешения экрана в списке доступных разрешений
    /// </summary>
    /// <param name="resolution">Строка указанного разрешения экрана</param>
    /// <returns>Индекс указанного разрешения экрана</returns>
    private int GetScreeResolutionIndex(string resolution)
    {
        for (int i = 0; i < _screenResolutions.Length; i++)
        {
            if (_screenResolutions[i].DisplayString == resolution)
                return i; // Возвращаем индекс, если совпадение найдено
        }
        return 0; // По умолчанию первый индекс
    }

    /// <summary>
    /// Метод для получения индекса указанной локализации в списке доступных локализаций
    /// </summary>
    /// <param name="localization">Строка указанной локализации</param>
    /// <returns>Индекс указанной локализации</returns>
    private int GetLocalizationIndex(string localization)
    {
        for (int i = 0; i < _localizations.Length; i++)
        {
            if (_localizations[i].Language == localization)
                return i; // Возвращаем индекс, если совпадение найдено
        }
        return 0; // По умолчанию первый индекс
    }

    /// <summary>
    /// Метод для обновления состояния чекбокса полноэкранного режима
    /// </summary>
    private void UpdateFullScreenToggle()
    {
        _fullScreenToggle.isOn = Settings.IsFullScreen; // Установка состояния чекбокса
    }

    /// <summary>
    /// Метод для обновления текущего значения слайдера громкости музыки
    /// </summary>
    private void UpdateMusicVolumeSlider()
    {
        _musicVolumeSlider.value = Settings.MusicVolume; // Установка значения слайдера громкости музыки
        UpdateMusicVolumeText(); // Обновление текста с громкостью
    }

    /// <summary>
    /// Метод для обновления текущего значения слайдера громкости звуков
    /// </summary>
    private void UpdateSoundVolumeSlider()
    {
        _soundVolumeSlider.value = Settings.SoundVolume; // Установка значения слайдера громкости звуков
        UpdateSoundVolumeText(); // Обновление текста с громкостью
    }

    /// <summary>
    /// Метод для обновления текстового поля с текущей громкостью музыки
    /// </summary>
    private void UpdateMusicVolumeText()
    {
        _musicVolumeText.text = $"{Settings.MusicVolume.ToString("F0")}"; // Обновляем текст, показывая громкость в процентах
    }

    /// <summary>
    /// Метод для обновления текстового поля с текущей громкостью звуков
    /// </summary>
    private void UpdateSoundVolumeText()
    {
        _soundVolumeText.text = $"{Settings.SoundVolume.ToString("F0")}"; // Обновляем текст, показывая громкость в процентах
    }

    /// <summary>
    /// Метод для обновления состояния чекбокса записи логов в файл
    /// </summary>
    private void UpdateFileLoggingToggle()
    {
        _fileLoggingToggle.isOn = Settings.IsFileLogging; // Установка состояния чекбокса
    }

    /// <summary>
    /// Метод для логирования ошибок
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private void LogError(string message)
    {
        Logger.Log(Logger.LogLevel.Error, nameof(SettingsMenuManager), message);
    }

    /// <summary>
    /// Метод для логирования предупреждений
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private void LogWarning(string message)
    {
        Logger.Log(Logger.LogLevel.Warning, nameof(SettingsMenuManager), message);
    }

    /// <summary>
    /// Метод для логирования информационных сообщений
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private void LogInfo(string message)
    {
        Logger.Log(Logger.LogLevel.Info, nameof(SettingsMenuManager), message);
    }

    #endregion
}
