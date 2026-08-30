using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Представление главного меню.
/// Отвечает за работу кнопок главного меню.
/// </summary>
public class MainMenuView : MonoBehaviour
{
    #region Инспектор

    /// <summary>
    /// Ссылка на меню настроек.
    /// </summary>
    [Header("Menu Windows")]
    [SerializeField]
    private SettingsMenuView _settingsMenu;

    /// <summary>
    /// Ссылка на меню создания игрока.
    /// </summary>
    [SerializeField]
    private PlayerCreationMenuView _playerCreationMenu;



    /// <summary>
    /// Экран загрузки.
    /// </summary>
    [SerializeField]
    private GameObject _loadingScreen;

    /// <summary>
    /// Контроллер загрузчика сцен.
    /// </summary>
    private SceneLoaderController _sceneLoaderController;



    /// <summary>
    /// Кнопка начала новой игры.
    /// </summary>
    [Header("Buttons")]
    [SerializeField]
    private Button _newGameButton;

    /// <summary>
    /// Кнопка продолжения ранее сохранённой игры.
    /// </summary>
    [SerializeField]
    private Button _continueButton;

    /// <summary>
    /// Кнопка открытия меню настроек.
    /// </summary>
    [SerializeField]
    private Button _settingsButton;

    /// <summary>
    /// Кнопка выхода из игры.
    /// </summary>
    [SerializeField]
    private Button _quitGameButton;

    #endregion



    #region Unity Lifecycle

    /// <summary>
    /// Метод Unity вызывается при старте объекта.
    /// Выполняет первичную настройку главного меню.
    /// </summary>
    private void Awake()
    {
        LogInfo("Main menu initialization started.");

        _sceneLoaderController = new SceneLoaderController(this, _loadingScreen);

        HideSettingsMenu();
        HidePlayerCreationMenu();
        UpdateContinueButton();
        BindEvents();

        LogInfo("Main menu initialized.");
    }

    /// <summary>
    /// Метод Unity вызывается при уничтожении объекта.
    /// Отписывает события от кнопок.
    /// </summary>
    private void OnDestroy()
    {
        UnbindEvents();

        LogInfo("Main menu destroyed, events unbound.");
    }

    #endregion



    #region Привязка событий

    /// <summary>
    /// Метод подписывает кнопки на обработчики.
    /// </summary>
    private void BindEvents()
    {
        if (_newGameButton != null)
            _newGameButton.onClick.AddListener(OnNewGameClicked);

        if (_continueButton != null)
            _continueButton.onClick.AddListener(OnContinueClicked);

        if (_settingsButton != null)
            _settingsButton.onClick.AddListener(OnSettingsClicked);

        if (_quitGameButton != null)
            _quitGameButton.onClick.AddListener(OnQuitGameClicked);

        LogInfo("Main menu UI events successfully bound.");
    }

    /// <summary>
    /// Метод отписывает кнопки от обработчиков.
    /// </summary>
    private void UnbindEvents()
    {
        if (_newGameButton != null)
            _newGameButton.onClick.RemoveListener(OnNewGameClicked);

        if (_continueButton != null)
            _continueButton.onClick.RemoveListener(OnContinueClicked);

        if (_settingsButton != null)
            _settingsButton.onClick.RemoveListener(OnSettingsClicked);

        if (_quitGameButton != null)
            _quitGameButton.onClick.RemoveListener(OnQuitGameClicked);
    }

    #endregion



    #region Вспомогательные методы

    /// <summary>
    /// Метод показывает меню настроек.
    /// </summary>
    private void ShowSettingsMenu()
    {
        if (_settingsMenu == null)
        {
            LogWarning("Settings menu is not assigned.");
            return;
        }

        _settingsMenu.ShowSettingsMenu();
        LogInfo("Settings menu shown.");
    }

    /// <summary>
    /// Метод скрывает меню настроек.
    /// </summary>
    private void HideSettingsMenu()
    {
        if (_settingsMenu == null)
        {
            LogWarning("Settings menu is not assigned.");
            return;
        }

        _settingsMenu.HideSettingsMenu();
        LogInfo("Settings menu hidden.");
    }



    /// <summary>
    /// Метод показывает меню создания игрока.
    /// </summary>
    private void ShowPlayerCreationMenu()
    {
        if (_playerCreationMenu == null)
        {
            LogWarning("Player creation menu is not assigned.");
            return;
        }

        _playerCreationMenu.ShowPlayerCreationMenu();
        LogInfo("Player creation menu shown.");
    }

    /// <summary>
    /// Метод скрывает меню создания игрока.
    /// </summary>
    private void HidePlayerCreationMenu()
    {
        if (_playerCreationMenu == null)
        {
            LogWarning("Player creation menu is not assigned.");
            return;
        }

        _playerCreationMenu.HidePlayerCreationMenu();
        LogInfo("Player creation menu hidden.");
    }



    /// <summary>
    /// Метод обновляет состояние кнопки "Продолжить" в зависимости от наличия сохранения игры.
    /// </summary>
    private void UpdateContinueButton()
    {
        if (_continueButton == null)
        {
            LogWarning("Continue button is not assigned.");
            return;
        }

        bool isSaveExists = GameSaveService.IsGameSaveFileExists;
        _continueButton.interactable = isSaveExists;

        LogInfo($"Continue button state updated. Save exists: {isSaveExists}.");
    }

    #endregion



    #region Обработчики событий

    /// <summary>
    /// Обработчик нажатия на кнопку начала новой игры.
    /// </summary>
    private void OnNewGameClicked()
    {
        LogInfo("New Game button clicked.");

        try
        {
            ShowPlayerCreationMenu();
        }
        catch (System.Exception exception)
        {
            LogError($"Failed to start new game: {exception.Message}");
        }
    }

    /// <summary>
    /// Обработчик нажатия на кнопку продолжения ранее сохранённой игры.
    /// </summary>
    private void OnContinueClicked()
    {
        LogInfo("Continue button clicked.");

        try
        {
            if (!GameSaveService.Load())
            {
                LogWarning("Game save was not found or failed to load. Continue aborted.");
                UpdateContinueButton();
                return;
            }

            string sceneName = GameSaveService.SceneName;

            if (string.IsNullOrWhiteSpace(sceneName))
            {
                LogWarning("Game save scene name is empty. Continue aborted.");
                return;
            }

            if (_sceneLoaderController == null)
            {
                LogWarning("Scene loader controller is not assigned. Continue aborted.");
                return;
            }

            LogInfo($"Loading saved scene: {sceneName}");
            _sceneLoaderController.LoadScene(sceneName);
        }
        catch (System.Exception exception)
        {
            LogError($"Failed to continue game: {exception.Message}");
        }
    }

    /// <summary>
    /// Обработчик нажатия на кнопку открытия меню настроек.
    /// </summary>
    private void OnSettingsClicked()
    {
        LogInfo("Settings button clicked.");

        ShowSettingsMenu();
    }

    /// <summary>
    /// Обработчик нажатия на кнопку выхода из игры.
    /// </summary>
    private void OnQuitGameClicked()
    {
        LogInfo("Quit game button clicked.");

        try
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            LogInfo("Play mode stopped in Unity Editor.");
#else
            Application.Quit();
            LogInfo("Application quit requested.");
#endif
        }
        catch (System.Exception exception)
        {
            LogError($"Failed to quit application: {exception.Message}");
        }
    }

    #endregion



    #region Логирование

    /// <summary>
    /// Метод записывает информационное сообщение в лог.
    /// </summary>
    private void LogInfo(string message)
    {
        Logger.Log(LogLevel.Info, nameof(MainMenuView), message);
    }

    /// <summary>
    /// Метод записывает предупреждение в лог.
    /// </summary>
    private void LogWarning(string message)
    {
        Logger.Log(LogLevel.Warning, nameof(MainMenuView), message);
    }

    /// <summary>
    /// Метод записывает ошибку в лог.
    /// </summary>
    private void LogError(string message)
    {
        Logger.Log(LogLevel.Error, nameof(MainMenuView), message);
    }

    #endregion
}