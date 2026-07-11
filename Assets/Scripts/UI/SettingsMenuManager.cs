using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Управляет только интерфейсом меню настроек.
/// Не занимается загрузкой настроек из файлов и не хранит бизнес-логику.
/// </summary>
public class SettingsMenuManager : MonoBehaviour
{
    #region Инспектор

    [Header("Controls")]
    /// <summary>
    /// Выпадающий список разрешения экрана.
    /// </summary>
    [SerializeField] private TMP_Dropdown screenResolutionDropdown;

    /// <summary>
    /// Выпадающий список локализации.
    /// </summary>
    [SerializeField] private TMP_Dropdown localizationDropdown;

    /// <summary>
    /// Переключатель полноэкранного режима.
    /// </summary>
    [SerializeField] private Toggle fullScreenToggle;

    /// <summary>
    /// Переключатель файлового логирования.
    /// </summary>
    [SerializeField] private Toggle fileLoggingToggle;

    /// <summary>
    /// Слайдер громкости музыки.
    /// </summary>
    [SerializeField] private Slider musicVolumeSlider;

    /// <summary>
    /// Слайдер громкости звуков.
    /// </summary>
    [SerializeField] private Slider soundVolumeSlider;

    /// <summary>
    /// Текстовое отображение громкости музыки.
    /// </summary>
    [SerializeField] private TMP_Text musicVolumeText;

    /// <summary>
    /// Текстовое отображение громкости звуков.
    /// </summary>
    [SerializeField] private TMP_Text soundVolumeText;

    /// <summary>
    /// Кнопка "Назад" в меню настроек.
    /// </summary>
    [SerializeField] private Button backButton;

    [Header("Audio")]
    /// <summary>
    /// Источник музыки.
    /// </summary>
    [SerializeField] private AudioSource musicSource;

    /// <summary>
    /// Источники игровых звуков.
    /// </summary>
    [SerializeField] private AudioSource[] soundSources;


    [Header("Localization Targets")]
    /// <summary>
    /// Набор UI-элементов, которые нужно локализовать.
    /// </summary>
    [SerializeField] private List<LocalizedTextBinding> localizedTexts;

    [Header("Options")]
    /// <summary>
    /// Доступные разрешения экрана.
    /// </summary>
    [SerializeField]
    private List<ScreenResolutionOption> screenResolutions = new List<ScreenResolutionOption>()
    {
        new ScreenResolutionOption(1920, 1080),
        new ScreenResolutionOption(1600, 900),
        new ScreenResolutionOption(1366, 768),
        new ScreenResolutionOption(1280, 720),
        new ScreenResolutionOption(2560, 1440),
        new ScreenResolutionOption(3840, 2160),
        new ScreenResolutionOption(640, 360)
    };

    /// <summary>
    /// Доступные языки локализации.
    /// </summary>
    [SerializeField]
    private List<LocalizationOption> localizations = new List<LocalizationOption>()
    {
        new LocalizationOption("English", "EN"),
        new LocalizationOption("Русский", "RU")
    };

    #endregion

    # region Состояние

    /// <summary>
    /// Флаг, были ли изменены какие-либо настройки после открытия меню.
    /// Если true — кнопка Back должна быть неактивна.
    /// </summary>
    private bool settingsChanged;

    #endregion

    #region Внутренние типы

    /// <summary>
    /// Связка ключа локализации и текстового поля.
    /// </summary>
    [Serializable]
    public class LocalizedTextBinding
    {
        public string Key;
        public TMP_Text Text;
    }

    /// <summary>
    /// Описание одного разрешения экрана.
    /// </summary>
    [Serializable]
    public struct ScreenResolutionOption
    {
        public int Width;
        public int Height;

        public ScreenResolutionOption(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Возвращает строковое представление разрешения в формате "1920x1080".
        /// </summary>
        public string DisplayName => $"{Width}x{Height}";
    }

    /// <summary>
    /// Описание одного языка локализации.
    /// </summary>
    [Serializable]
    public struct LocalizationOption
    {
        public string Name;
        public string Code;

        public LocalizationOption(string name, string code)
        {
            Name = name;
            Code = code;
        }
    }

    #endregion


    #region Публичные методы

    /// <summary>
    /// Инициализирует UI меню настроек.
    /// Вызывается после того, как настройки уже загружены bootstrapper-ом.
    /// </summary>
    public void Initialize()
    {
        RefreshUI();
        ApplyAllSettingsToScene();
        LogInfo("Settings menu initialized.");
    }

    /// <summary>
    /// Обновляет все элементы интерфейса согласно текущим значениям SettingsService.
    /// </summary>
    public void RefreshUI()
    {
        UpdateResolutionDropdown();
        UpdateLocalizationDropdown();
        UpdateFullscreenToggle();
        UpdateFileLoggingToggle();
        UpdateMusicVolumeSlider();
        UpdateSoundVolumeSlider();
    }

    /// <summary>
    /// Кнопка "Apply".
    /// Сохраняет текущие настройки и применяет их.
    /// </summary>
    public void ApplyButtonClick()
    {
        SettingsService.Save();
        ApplyAllSettingsToScene();
        ResetSettingsChanged();
        LogInfo("Settings applied and saved.");
    }

    /// <summary>
    /// Кнопка "Cancel".
    /// Возвращает сохранённые настройки и обновляет UI.
    /// </summary>
    public void CancelButtonClick()
    {
        SettingsService.Load();
        RefreshUI();
        ApplyAllSettingsToScene();
        ResetSettingsChanged();
        LogInfo("Settings changes canceled.");
    }

    /// <summary>
    /// Обработчик изменения состояния fullscreen.
    /// </summary>
    public void OnFullScreenToggleChanged()
    {
        if (fullScreenToggle == null)
        {
            return;
        }

        SettingsService.IsFullScreen = fullScreenToggle.isOn;
        SettingsService.ApplyFullScreen();
        SettingsChanged();
    }

    /// <summary>
    /// Обработчик изменения файлового логирования.
    /// </summary>
    public void OnFileLoggingToggleChanged()
    {
        if (fileLoggingToggle == null)
        {
            return;
        }

        SettingsService.IsFileLogging = fileLoggingToggle.isOn;
        SettingsService.ApplyFileLogging();
        SettingsChanged();
    }

    /// <summary>
    /// Обработчик изменения громкости музыки.
    /// </summary>
    public void OnMusicVolumeChanged()
    {
        if (musicVolumeSlider == null)
        {
            return;
        }

        SettingsService.MusicVolume = musicVolumeSlider.value;
        SettingsService.ApplyMusicVolume(musicSource);
        UpdateMusicVolumeText();
        SettingsChanged();
    }

    /// <summary>
    /// Обработчик изменения громкости звуков.
    /// </summary>
    public void OnSoundVolumeChanged()
    {
        if (soundVolumeSlider == null)
        {
            return;
        }

        SettingsService.SoundVolume = soundVolumeSlider.value;
        ApplySoundVolumeToAllSources();
        UpdateSoundVolumeText();
        SettingsChanged();
    }

    /// <summary>
    /// Обработчик выбора разрешения экрана.
    /// </summary>
    public void OnScreenResolutionChanged(int index)
    {
        if (!IsValidResolutionIndex(index))
        {
            LogError("Invalid resolution index.");
            return;
        }

        SettingsService.ScreenResolution = screenResolutions[index].DisplayName;
        SettingsService.ApplyScreenResolution();
        SettingsChanged();
    }

    /// <summary>
    /// Обработчик выбора языка локализации.
    /// </summary>
    public void OnLocalizationChanged(int index)
    {
        if (!IsValidLocalizationIndex(index))
        {
            LogError("Invalid localization index.");
            return;
        }

        LocalizationOption option = localizations[index];
        SettingsService.Localization = option.Name;
        SettingsService.LanguageCode = option.Code;

        ApplyLocalizationToUI();
        SettingsChanged();
    }

    #endregion


    #region Применение настроек

    /// <summary>
    /// Применяет все текущие настройки к системе и UI.
    /// </summary>
    private void ApplyAllSettingsToScene()
    {
        SettingsService.ApplyToSystem();
        SettingsService.ApplyMusicVolume(musicSource);
        ApplySoundVolumeToAllSources();
        ApplyLocalizationToUI();
    }

    /// <summary>
    /// Применяет локализацию ко всем привязанным текстовым элементам.
    /// </summary>
    private void ApplyLocalizationToUI()
    {
        Dictionary<string, TMP_Text> mapping = CreateLocalizationMap();
        SettingsService.ApplyLocalization(mapping);
    }

    /// <summary>
    /// Создаёт словарь ключей и текстовых компонентов для локализации.
    /// </summary>
    /// <returns>Словарь локализуемых элементов.</returns>
    private Dictionary<string, TMP_Text> CreateLocalizationMap()
    {
        Dictionary<string, TMP_Text> result = new Dictionary<string, TMP_Text>();

        if (localizedTexts == null)
        {
            return result;
        }

        foreach (LocalizedTextBinding binding in localizedTexts)
        {
            if (binding == null || binding.Text == null || string.IsNullOrWhiteSpace(binding.Key))
            {
                continue;
            }

            result[binding.Key] = binding.Text;
        }

        return result;
    }

    /// <summary>
    /// Применяет громкость ко всем источникам звука.
    /// </summary>
    private void ApplySoundVolumeToAllSources()
    {
        if (soundSources == null)
        {
            return;
        }

        foreach (AudioSource source in soundSources)
        {
            if (source != null)
            {
                SettingsService.ApplySoundVolume(source);
            }
        }
    }

    #endregion

    #region Изменение настроек

    /// <summary>
    /// Помечает, что настройки были изменены пользователем.
    /// </summary>
    private void SettingsChanged()
    {
        settingsChanged = true;
        UpdateBackButton();
    }

    /// <summary>
    /// Сбрасывает флаг изменений настроек.
    /// Вызывать после Apply или Cancel.
    /// </summary>
    private void ResetSettingsChanged()
    {
        settingsChanged = false;
        UpdateBackButton();
    }

    /// <summary>
    /// Обновляет состояние кнопки "Назад" в зависимости от того,
    /// были ли изменены настройки.
    /// </summary>
    private void UpdateBackButton()
    {
        if (backButton == null)
        {
            LogWarning("Back button not found.");
            return;
        }

        // Если настройки менялись, назад нельзя.
        backButton.interactable = !settingsChanged;
    }

    #endregion

    #region Обновление UI

    // <summary>
    /// Заполняет dropdown разрешений и устанавливает выбранное значение без вызова событий.
    /// </summary>
    private void UpdateResolutionDropdown()
    {
        if (screenResolutionDropdown == null)
        {
            return;
        }

        screenResolutionDropdown.onValueChanged.RemoveListener(OnScreenResolutionChanged);
        screenResolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        foreach (ScreenResolutionOption option in screenResolutions)
        {
            options.Add(option.DisplayName);
        }

        screenResolutionDropdown.AddOptions(options);
        screenResolutionDropdown.SetValueWithoutNotify(GetCurrentResolutionIndex());
        screenResolutionDropdown.RefreshShownValue();
        screenResolutionDropdown.onValueChanged.AddListener(OnScreenResolutionChanged);
    }

    /// <summary>
    /// Заполняет dropdown локализаций и устанавливает выбранное значение без вызова событий.
    /// </summary>
    private void UpdateLocalizationDropdown()
    {
        if (localizationDropdown == null)
        {
            return;
        }

        localizationDropdown.onValueChanged.RemoveListener(OnLocalizationChanged);
        localizationDropdown.ClearOptions();

        List<string> options = new List<string>();
        foreach (LocalizationOption option in localizations)
        {
            options.Add(option.Name);
        }

        localizationDropdown.AddOptions(options);
        localizationDropdown.SetValueWithoutNotify(GetCurrentLocalizationIndex());
        localizationDropdown.RefreshShownValue();
        localizationDropdown.onValueChanged.AddListener(OnLocalizationChanged);
    }

    /// <summary>
    /// Синхронизирует переключатель fullscreen без вызова события.
    /// </summary>
    private void UpdateFullscreenToggle()
    {
        if (fullScreenToggle != null)
        {
            fullScreenToggle.SetIsOnWithoutNotify(SettingsService.IsFullScreen);
        }
    }

    /// <summary>
    /// Синхронизирует переключатель файлового логирования без вызова события.
    /// </summary>
    private void UpdateFileLoggingToggle()
    {
        if (fileLoggingToggle != null)
        {
            fileLoggingToggle.SetIsOnWithoutNotify(SettingsService.IsFileLogging);
        }
    }

    /// <summary>
    /// Синхронизирует слайдер громкости музыки без вызова события.
    /// </summary>
    private void UpdateMusicVolumeSlider()
    {
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.SetValueWithoutNotify(SettingsService.MusicVolume);
            UpdateMusicVolumeText();
        }
    }

    /// <summary>
    /// Синхронизирует слайдер громкости звуков без вызова события.
    /// </summary>
    private void UpdateSoundVolumeSlider()
    {
        if (soundVolumeSlider != null)
        {
            soundVolumeSlider.SetValueWithoutNotify(SettingsService.SoundVolume);
            UpdateSoundVolumeText();
        }
    }

    /// <summary>
    /// Обновляет текстовое отображение громкости музыки.
    /// </summary>
    private void UpdateMusicVolumeText()
    {
        if (musicVolumeText != null)
        {
            musicVolumeText.text = Mathf.RoundToInt(SettingsService.MusicVolume).ToString();
        }
    }

    /// <summary>
    /// Обновляет текстовое отображение громкости звуков.
    /// </summary>
    private void UpdateSoundVolumeText()
    {
        if (soundVolumeText != null)
        {
            soundVolumeText.text = Mathf.RoundToInt(SettingsService.SoundVolume).ToString();
        }
    }

    #endregion


    #region Вспомогательные методы

    /// <summary>
    /// Возвращает индекс текущего разрешения в списке.
    /// </summary>
    private int GetCurrentResolutionIndex()
    {
        for (int index = 0; index < screenResolutions.Count; index++)
        {
            if (screenResolutions[index].DisplayName == SettingsService.ScreenResolution)
            {
                return index;
            }
        }

        return 0;
    }

    /// <summary>
    /// Возвращает индекс текущей локализации в списке.
    /// </summary>
    private int GetCurrentLocalizationIndex()
    {
        for (int index = 0; index < localizations.Count; index++)
        {
            if (localizations[index].Name == SettingsService.Localization)
            {
                return index;
            }
        }

        return 0;
    }

    /// <summary>
    /// Проверяет, существует ли разрешение по переданному индексу.
    /// </summary>
    private bool IsValidResolutionIndex(int index)
    {
        return index >= 0 && index < screenResolutions.Count;
    }

    /// <summary>
    /// Проверяет, существует ли локализация по переданному индексу.
    /// </summary>
    private bool IsValidLocalizationIndex(int index)
    {
        return index >= 0 && index < localizations.Count;
    }

    #endregion


    #region Логирование

    /// <summary>
    /// Записывает информационное сообщение в лог.
    /// </summary>
    private void LogInfo(string message)
    {
        Logger.Log(LogLevel.Info, nameof(SettingsMenuManager), message);
    }

    /// <summary>
    /// Записывает предупреждение.
    /// </summary>
    private void LogWarning(string message)
    {
        Logger.Log(LogLevel.Warning, nameof(SettingsMenuManager), message);
    }

    /// <summary>
    /// Записывает предупреждение в лог.
    /// </summary>
    private void LogError(string message)
    {
        Logger.Log(LogLevel.Error, nameof(SettingsMenuManager), message);
    }

    #endregion
}
