using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Представление меню настроек.
/// Отвечает за работу с UI-элементами.
/// </summary>
public class SettingsMenuView : MonoBehaviour
{
    #region Инспектор

    /// <summary>
    /// Ссылка на контроллер меню настроек.
    /// </summary>
    private SettingsMenuController _controller = new SettingsMenuController();



    /// <summary>
    /// Окно меню настроек.
    /// </summary>
    [Header("Menu Window")]
    [SerializeField]
    private GameObject _settingsMenu;



    /// <summary>
    /// Выпадающий список разрешений экрана.
    /// </summary>
    [Header("Screen Settings")]
    [SerializeField]
    private TMP_Dropdown _screenResolutionDropdown;

    /// <summary>
    /// Доступные разрешения экрана.
    /// </summary>
    [SerializeField]
    private List<ScreenResolutionOption> _screenResolutions = new List<ScreenResolutionOption>()
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
    /// Переключатель полноэкранного режима.
    /// </summary>
    [SerializeField]
    private Toggle _fullScreenToggle;



    /// <summary>
    /// Слайдер громкости музыки.
    /// </summary>
    [Header("Audio Settings")]
    [SerializeField]
    private Slider _musicVolumeSlider;

    /// <summary>
    /// Текст для отображения громкости музыки.
    /// </summary>
    [SerializeField]
    private TMP_Text _musicVolumeText;

    /// <summary>
    /// Источник музыки.
    /// </summary>
    [SerializeField]
    private AudioSource _musicSource;



    /// <summary>
    /// Слайдер громкости звуков.
    /// </summary>
    [SerializeField]
    private Slider _soundVolumeSlider;

    /// <summary>
    /// Текст для отображения громкости звуков.
    /// </summary>
    [SerializeField]
    private TMP_Text _soundVolumeText;

    /// <summary>
    /// Источники звуков.
    /// </summary>
    [SerializeField]
    private AudioSource[] _soundSources;



    /// <summary>
    /// Выпадающий список локализаций.
    /// </summary>
    [Header("Localization Settings")]
    [SerializeField]
    private TMP_Dropdown _localizationDropdown;

    /// <summary>
    /// Доступные локализации.
    /// </summary>
    [SerializeField]
    private List<LocalizationOption> _localizations = new List<LocalizationOption>()
    {
        new LocalizationOption("English", "EN"),
        new LocalizationOption("Русский", "RU")
    };

    /// <summary>
    /// Локализуемые UI-элементы.
    /// </summary>
    [SerializeField]
    private List<LocalizationTarget> _localizationTargets = new List<LocalizationTarget>();



    /// <summary>
    /// Переключатель записи логов в файл.
    /// </summary>
    [Header("Logging Settings")]
    [SerializeField]
    private Toggle _fileLoggingToggle;



    /// <summary>
    /// Кнопка сохранения настроек.
    /// </summary>
    [Header("Buttons")]
    [SerializeField]
    private Button _saveButton;

    /// <summary>
    /// Кнопка отмены изменений.
    /// </summary>
    [SerializeField]
    private Button _cancelButton;

    /// <summary>
    /// Кнопка закрытия меню настроек.
    /// </summary>
    [SerializeField]
    private Button _closeButton;

    #endregion



    #region Unity Lifecycle

    /// <summary>
    /// Метод Unity вызывается при старте объекта.
    /// Выполняет первичную настройку меню настроек.
    /// </summary>
    private void Awake()
    {
        LogInfo("Initializing settings menu.");

        if (_controller == null)
        {
            _controller = new SettingsMenuController();
        }

        _controller.InitializeSettings(_localizationTargets, _musicSource, _soundSources);

        InitializeDropdowns();

        UpdateUI();
        UpdateButtons();

        BindEvents();

        LogInfo("Settings menu initialized.");
    }

    /// <summary>
    /// Метод Unity вызывается при уничтожении объекта.
    /// Отписывает события от кнопок.
    /// </summary>
    private void OnDestroy()
    {
        UnbindEvents();

        LogInfo("Settings menu destroyed, events unbound.");
    }

    #endregion



    #region Отображение меню

    /// <summary>
    /// Метод отображает меню настроек.
    /// </summary>
    public void ShowSettingsMenu()
    {
        if (_settingsMenu == null)
        {
            LogWarning("Settings menu window is not assigned.");
            return;
        }

        _settingsMenu.SetActive(true);
        LogInfo("Settings menu window shown.");
    }

    /// <summary>
    /// Метод скрывает меню настроек.
    /// </summary>
    public void HideSettingsMenu()
    {
        if (_settingsMenu == null)
        {
            LogWarning("Settings menu window is not assigned.");
            return;
        }

        _settingsMenu.SetActive(false);
        LogInfo("Settings menu window hidden.");
    }

    #endregion



    #region Инициализация

    /// <summary>
    /// Метод заполняет выпадающие списки доступными вариантами.
    /// </summary>
    private void InitializeDropdowns()
    {
        if (_screenResolutionDropdown != null)
        {
            _screenResolutionDropdown.ClearOptions();

            List<string> resolutionOptions = new List<string>();
            for (int i = 0; i < _screenResolutions.Count; i++)
            {
                resolutionOptions.Add(_screenResolutions[i].DisplayName);
            }

            _screenResolutionDropdown.AddOptions(resolutionOptions);
        }
        else
        {
            LogWarning("Screen resolution dropdown is not assigned.");
        }



        if (_localizationDropdown != null)
        {
            _localizationDropdown.ClearOptions();

            List<string> localizationOptions = new List<string>();
            for (int i = 0; i < _localizations.Count; i++)
            {
                localizationOptions.Add(_localizations[i].Name);
            }

            _localizationDropdown.AddOptions(localizationOptions);
        }
        else
        {
            LogWarning("Localization dropdown is not assigned.");
        }
    }

    #endregion



    #region Привязка событий

    /// <summary>
    /// Метод подписывает UI-элементы на обработчики.
    /// </summary>
    private void BindEvents()
    {
        if (_screenResolutionDropdown != null)
            _screenResolutionDropdown.onValueChanged.AddListener(OnScreenResolutionChanged);

        if (_fullScreenToggle != null)
            _fullScreenToggle.onValueChanged.AddListener(OnFullScreenChanged);

        if (_musicVolumeSlider != null)
            _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

        if (_soundVolumeSlider != null)
            _soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeChanged);

        if (_localizationDropdown != null)
            _localizationDropdown.onValueChanged.AddListener(OnLocalizationChanged);

        if (_fileLoggingToggle != null)
            _fileLoggingToggle.onValueChanged.AddListener(OnFileLoggingChanged);

        if (_saveButton != null)
            _saveButton.onClick.AddListener(OnSaveClicked);

        if (_cancelButton != null)
            _cancelButton.onClick.AddListener(OnCancelClicked);

        if (_closeButton != null)
            _closeButton.onClick.AddListener(OnCloseClicked);

        LogInfo("UI events successfully bound.");
    }

    /// <summary>
    /// Метод отписывает UI-элементы от обработчиков.
    /// </summary>
    private void UnbindEvents()
    {
        if (_screenResolutionDropdown != null)
            _screenResolutionDropdown.onValueChanged.RemoveListener(OnScreenResolutionChanged);

        if (_fullScreenToggle != null)
            _fullScreenToggle.onValueChanged.RemoveListener(OnFullScreenChanged);

        if (_musicVolumeSlider != null)
            _musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);

        if (_soundVolumeSlider != null)
            _soundVolumeSlider.onValueChanged.RemoveListener(OnSoundVolumeChanged);

        if (_localizationDropdown != null)
            _localizationDropdown.onValueChanged.RemoveListener(OnLocalizationChanged);

        if (_fileLoggingToggle != null)
            _fileLoggingToggle.onValueChanged.RemoveListener(OnFileLoggingChanged);

        if (_saveButton != null)
            _saveButton.onClick.RemoveListener(OnSaveClicked);

        if (_cancelButton != null)
            _cancelButton.onClick.RemoveListener(OnCancelClicked);

        if (_closeButton != null)
            _closeButton.onClick.RemoveListener(OnCloseClicked);
    }

    #endregion



    #region Обработчики событий

    /// <summary>
    /// Обработчик обновляет выбранное разрешение экрана внутри временных данных.
    /// </summary>
    private void OnScreenResolutionChanged(int index)
    {
        if (index < 0 || index >= _screenResolutions.Count)
        {
            LogWarning($"Invalid resolution index: {index}.");
            return;
        }

        if (_controller != null)
        {
            ScreenResolutionOption option = _screenResolutions[index];
            _controller.TempSettingsData.ScreenResolution = $"{option.Width}x{option.Height}";

            LogInfo($"Screen resolution changed: {_controller.TempSettingsData.ScreenResolution}.");

            _controller.ApplyScreenResolution();
            UpdateButtons();
        }
        else
        {
            LogWarning("SettingsMenuController is not assigned.");
        }
    }

    /// <summary>
    /// Обработчик обновляет состояние полноэкранного режима внутри временных данных.
    /// </summary>
    private void OnFullScreenChanged(bool isFullScreen)
    {
        if (_controller != null)
        {
            _controller.TempSettingsData.IsFullScreen = isFullScreen;

            LogInfo($"Fullscreen mode changed: {isFullScreen}.");

            _controller.ApplyFullScreen();
            UpdateButtons();
        }
        else
        {
            LogWarning("SettingsMenuController is not assigned.");
        }
    }

    /// <summary>
    /// Обработчик обновляет громкость музыки внутри временных данных.
    /// </summary>
    private void OnMusicVolumeChanged(float value)
    {
        if (_controller != null)
        {
            _controller.TempSettingsData.MusicVolume = value;
            UpdateMusicVolumeText(value);

            LogInfo($"Music volume changed: {value:0.00}.");

            _controller.ApplyMusicVolume(_musicSource);
            UpdateButtons();
        }
        else
        {
            LogWarning("SettingsMenuController is not assigned.");
        }
    }

    /// <summary>
    /// Обработчик обновляет громкость звуков внутри временных данных.
    /// </summary>
    private void OnSoundVolumeChanged(float value)
    {
        if (_controller != null)
        {
            _controller.TempSettingsData.SoundVolume = value;
            UpdateSoundVolumeText(value);

            LogInfo($"Sound volume changed: {value:0.00}.");

            _controller.ApplySoundVolume(_soundSources);
            UpdateButtons();
        }
        else
        {
            LogWarning("SettingsMenuController is not assigned.");
        }
    }

    /// <summary>
    /// Обработчик обновляет локализацию внутри временных данных.
    /// </summary>
    private void OnLocalizationChanged(int index)
    {
        if (index < 0 || index >= _localizations.Count)
        {
            LogWarning($"Invalid localization index: {index}.");
            return;
        }

        if (_controller != null)
        {
            _controller.TempSettingsData.Localization = _localizations[index].Name;
            _controller.TempSettingsData.LanguageCode = _localizations[index].Code;

            LogInfo($"Localization changed: {_localizations[index].Name} ({_localizations[index].Code}).");

            _controller.ApplyLocalization();
            UpdateButtons();
        }
        else
        {
            LogWarning("SettingsMenuController is not assigned.");
        }
    }

    /// <summary>
    /// Обработчик обновляет состояние записи логов в файл внутри временных данных.
    /// </summary>
    private void OnFileLoggingChanged(bool isOn)
    {
        if (_controller != null)
        {
            _controller.TempSettingsData.IsFileLogging = isOn;

            LogInfo($"File logging changed: {isOn}.");

            _controller.ApplyFileLogging();
            UpdateButtons();
        }
        else
        {
            LogWarning("SettingsMenuController is not assigned.");
        }
    }



    /// <summary>
    /// Обработчик нажатия на кнопку сохранения настроек.
    /// </summary>
    private void OnSaveClicked()
    {
        if (_controller == null)
        {
            LogWarning("SettingsMenuController is not assigned.");
            return;
        }

        LogInfo("Save button clicked.");

        _controller.ApplySettings();
        SettingsService.Save();

        _controller.ResetSettingsChanged();
        UpdateButtons();

        LogInfo("Settings saved and change flag reset.");
    }

    /// <summary>
    /// Обработчик нажатие на кнопку отмены изменений.
    /// </summary>
    private void OnCancelClicked()
    {
        if (_controller == null)
        {
            LogWarning("SettingsMenuController is not assigned.");
            return;
        }

        LogInfo("Cancel button clicked.");

        _controller.RestoreSettings();

        UpdateUI();

        _controller.ResetSettingsChanged();
        UpdateButtons();

        LogInfo("Changes canceled, UI restored to saved settings.");
    }

    /// <summary>
    /// Обработчик нажатия на кнопку закрытия меню.
    /// </summary>
    private void OnCloseClicked()
    {
        LogInfo("Close button clicked.");

        HideSettingsMenu();
    }

    #endregion



    #region Обновление UI

    /// <summary>
    /// Метод обновляет все UI-элементы из временных данных.
    /// </summary>
    private void UpdateUI()
    {
        if (_controller == null)
        {
            LogWarning("SettingsMenuController is not assigned.");
            return;
        }

        SettingsData tempSettingsData = _controller.TempSettingsData;

        UpdateScreenResolution(tempSettingsData.ScreenResolution);
        UpdateFullScreen(tempSettingsData.IsFullScreen);
        UpdateMusicVolume(tempSettingsData.MusicVolume);
        UpdateSoundVolume(tempSettingsData.SoundVolume);
        UpdateLocalization(tempSettingsData.LanguageCode);
        UpdateFileLogging(tempSettingsData.IsFileLogging);
    }



    /// <summary>
    /// Метод устанавливает выбранное разрешение экрана в выпадающий список.
    /// </summary>
    private void UpdateScreenResolution(string screenResolution)
    {
        if (_screenResolutionDropdown == null)
            return;

        for (int i = 0; i < _screenResolutions.Count; i++)
        {
            if (_screenResolutions[i].DisplayName == screenResolution)
            {
                _screenResolutionDropdown.SetValueWithoutNotify(i);
                return;
            }
        }

        LogWarning($"Resolution not found for display: {screenResolution}.");
    }

    /// <summary>
    /// Метод устанавливает состояние полноэкранного режима в переключатель.
    /// </summary>
    private void UpdateFullScreen(bool isFullScreen)
    {
        if (_fullScreenToggle != null)
            _fullScreenToggle.SetIsOnWithoutNotify(isFullScreen);
    }

    /// <summary>
    /// Метод устанавливает громкость музыки в слайдер.
    /// </summary>
    private void UpdateMusicVolume(float volume)
    {
        if (_musicVolumeSlider != null)
            _musicVolumeSlider.SetValueWithoutNotify(volume);

        UpdateMusicVolumeText(volume);
    }

    /// <summary>
    /// Метод устанавливает громкость звуков в слайдер.
    /// </summary>
    private void UpdateSoundVolume(float volume)
    {
        if (_soundVolumeSlider != null)
            _soundVolumeSlider.SetValueWithoutNotify(volume);

        UpdateSoundVolumeText(volume);
    }

    /// <summary>
    /// Метод устанавливает выбранную локализацию в выпадающий список.
    /// </summary>
    private void UpdateLocalization(string localizationCode)
    {
        if (_localizationDropdown == null)
            return;

        for (int i = 0; i < _localizations.Count; i++)
        {
            if (_localizations[i].Code == localizationCode)
            {
                _localizationDropdown.SetValueWithoutNotify(i);
                return;
            }
        }

        LogWarning($"Localization not found for code: {localizationCode}.");
    }

    /// <summary>
    /// Метод устанавливает состояние записи логов в файл в переключатель.
    /// </summary>
    private void UpdateFileLogging(bool isFileLogging)
    {
        if (_fileLoggingToggle != null)
            _fileLoggingToggle.SetIsOnWithoutNotify(isFileLogging);
    }



    /// <summary>
    /// Метод обновляет текст громкости музыки.
    /// </summary>
    private void UpdateMusicVolumeText(float value)
    {
        if (_musicVolumeText != null)
            _musicVolumeText.text = Mathf.RoundToInt(value).ToString();
    }

    /// <summary>
    /// Метод обновляет текст громкости звуков.
    /// </summary>
    private void UpdateSoundVolumeText(float value)
    {
        if (_soundVolumeText != null)
            _soundVolumeText.text = Mathf.RoundToInt(value).ToString();
    }



    /// <summary>
    /// Метод обновляет активность кнопок в зависимости от того, были ли изменены настройки.
    /// </summary>
    private void UpdateButtons()
    {
        if (_controller == null)
        {
            LogWarning("SettingsMenuController is not assigned.");
            return;
        }

        bool isSettingsChanged = _controller.IsSettingsChanged;

        if (_saveButton != null)
            _saveButton.interactable = isSettingsChanged;

        if (_cancelButton != null)
            _cancelButton.interactable = isSettingsChanged;

        if (_closeButton != null)
            _closeButton.interactable = !isSettingsChanged;
    }

    #endregion



    #region Логирование

    /// <summary>
    /// Метод записывает информационное сообщение в лог.
    /// </summary>
    private void LogInfo(string message)
    {
        Logger.Log(LogLevel.Info, nameof(SettingsMenuView), message);
    }

    /// <summary>
    /// Метод записывает предупреждение в лог.
    /// </summary>
    private void LogWarning(string message)
    {
        Logger.Log(LogLevel.Warning, nameof(SettingsMenuView), message);
    }

    /// <summary>
    /// Метод записывает ошибку в лог.
    /// </summary>
    private void LogError(string message)
    {
        Logger.Log(LogLevel.Error, nameof(SettingsMenuView), message);
    }

    #endregion
}