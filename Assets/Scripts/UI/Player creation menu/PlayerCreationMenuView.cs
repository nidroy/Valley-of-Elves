using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Представление меню создания игрока.
/// Отвечает за работу с UI-элементами.
/// </summary>
public class PlayerCreationMenuView : MonoBehaviour
{
    #region Инспектор

    /// <summary>
    /// Ссылка на контроллер меню создания игрока.
    /// </summary>
    private PlayerCreationMenuController _controller = new PlayerCreationMenuController();



    /// <summary>
    /// Окно меню создания игрока.
    /// </summary>
    [Header("Menu Window")]
    [SerializeField]
    private GameObject _playerCreationMenu;



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
    /// Поле ввода имени игрока.
    /// </summary>
    [Header("Player Name")]
    [SerializeField]
    private TMP_InputField _playerNameInput;



    /// <summary>
    /// Поле отображения выбранного класса игрока.
    /// </summary>
    [Header("Player Class")]
    [SerializeField]
    private TMP_Text _playerClassText;

    /// <summary>
    /// Список карточек классов.
    /// </summary>
    [SerializeField]
    private List<PlayerClassCard> _classCards = new List<PlayerClassCard>();



    /// <summary>
    /// Кнопка продолжения создания игрока.
    /// </summary>
    [Header("Buttons")]
    [SerializeField]
    private Button _continueButton;

    /// <summary>
    /// Кнопка закрытия меню создания игрока.
    /// </summary>
    [SerializeField]
    private Button _closeButton;

    #endregion



    #region Unity Lifecycle

    /// <summary>
    /// Метод Unity вызывается при старте объекта.
    /// Выполняет первичную настройку меню создания игрока.
    /// </summary>
    private void Awake()
    {
        LogInfo("Initializing player creation menu.");

        if (_controller == null)
        {
            _controller = new PlayerCreationMenuController();
        }

        _sceneLoaderController = new SceneLoaderController(this, _loadingScreen);

        UpdateContinueButton();
        BindEvents();

        LogInfo("Player creation menu initialized.");
    }

    /// <summary>
    /// Метод Unity вызывается при уничтожении объекта.
    /// Отписывает события от UI-элементов.
    /// </summary>
    private void OnDestroy()
    {
        UnbindEvents();

        LogInfo("Player creation menu destroyed, events unbound.");
    }

    #endregion



    #region Отображение меню

    /// <summary>
    /// Метод отображает меню создания игрока.
    /// </summary>
    public void ShowPlayerCreationMenu()
    {
        if (_playerCreationMenu == null)
        {
            LogWarning("Player creation menu window is not assigned.");
            return;
        }

        _playerCreationMenu.SetActive(true);
        LogInfo("Player creation menu window shown.");
    }

    /// <summary>
    /// Метод скрывает меню создания игрока.
    /// </summary>
    public void HidePlayerCreationMenu()
    {
        if (_playerCreationMenu == null)
        {
            LogWarning("Player creation menu window is not assigned.");
            return;
        }

        _playerCreationMenu.SetActive(false);
        LogInfo("Player creation menu window hidden.");
    }

    #endregion



    #region Привязка событий

    /// <summary>
    /// Метод подписывает UI-элементы на обработчики.
    /// </summary>
    private void BindEvents()
    {
        if (_playerNameInput != null)
            _playerNameInput.onValueChanged.AddListener(OnPlayerNameChanged);

        if (_continueButton != null)
            _continueButton.onClick.AddListener(OnContinueClicked);

        if (_closeButton != null)
            _closeButton.onClick.AddListener(OnCloseClicked);

        if (_classCards != null)
        {
            for (int i = 0; i < _classCards.Count; i++)
            {
                int index = i;
                PlayerClassCard card = _classCards[index];

                if (card.SelectButton != null)
                    card.SelectButton.onClick.AddListener(() => OnClassSelectButtonClicked(index));
            }
        }

        LogInfo("UI events successfully bound.");
    }

    /// <summary>
    /// Метод отписывает UI-элементы от обработчиков.
    /// </summary>
    private void UnbindEvents()
    {
        if (_playerNameInput != null)
            _playerNameInput.onValueChanged.RemoveListener(OnPlayerNameChanged);

        if (_continueButton != null)
            _continueButton.onClick.RemoveListener(OnContinueClicked);

        if (_closeButton != null)
            _closeButton.onClick.RemoveListener(OnCloseClicked);

        if (_classCards != null)
        {
            for (int i = 0; i < _classCards.Count; i++)
            {
                int index = i;
                PlayerClassCard card = _classCards[index];

                if (card.SelectButton != null)
                    card.SelectButton.onClick.RemoveListener(() => OnClassSelectButtonClicked(index));
            }
        }

        LogInfo("UI events successfully unbound.");
    }

    #endregion



    #region Обработчики событий

    /// <summary>
    /// Обработчик изменения имени игрока.
    /// </summary>
    private void OnPlayerNameChanged(string value)
    {
        if (_controller != null)
        {
            _controller.ApplyPlayerName(value);

            LogInfo($"Player name changed: {value}");

            UpdateContinueButton();
        }
        else
        {
            LogWarning("PlayerCreationMenuController is not assigned.");
        }
    }

    /// <summary>
    /// Обработчик нажатия на кнопку выбора класса игрока.
    /// </summary>
    /// <param name="index">Индекс карточки.</param>
    private void OnClassSelectButtonClicked(int index)
    {
        if (_controller == null)
        {
            LogWarning("PlayerCreationMenuController is not assigned.");
            return;
        }

        if (_classCards == null || index < 0 || index >= _classCards.Count)
        {
            LogWarning($"Invalid class card index: {index}.");
            return;
        }

        PlayerClassCard card = _classCards[index];

        _controller.ApplyPlayerClass(card.ClassName, card.ClassText.text, card.MaxHealth, card.MaxMana);
        UpdatePlayerClassText(card.ClassText.text);

        LogInfo($"Class selected: {card.ClassName}");

        UpdateContinueButton();
    }



    /// <summary>
    /// Обработчик нажатия на кнопку продолжения.
    /// </summary>
    private void OnContinueClicked()
    {
        if (_controller == null)
        {
            LogWarning("PlayerCreationMenuController is not assigned.");
            return;
        }

        LogInfo("Continue button clicked.");

        _controller.ApplyPlayerData();
        PlayerSaveService.Save();

        GameSaveService.Default();
        GameSaveService.Save();

        try
        {
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
    /// Обработчик нажатия на кнопку закрытия меню.
    /// </summary>
    private void OnCloseClicked()
    {
        if (_controller == null)
        {
            LogWarning("PlayerCreationMenuController is not assigned.");
            return;
        }

        LogInfo("Close button clicked.");

        HidePlayerCreationMenu();
    }

    #endregion



    #region Обновление UI

    /// <summary>
    /// Метод обновляет активность кнопки продолжения в зависимости от состояния создания игрока.
    /// </summary>
    private void UpdateContinueButton()
    {
        if (_continueButton != null)
            _continueButton.interactable = _controller != null && _controller.IsPlayerCreated;
    }

    /// <summary>
    /// Обновляет текст выбранного класса игрока.
    /// </summary>
    /// <param name="className">Название класса.</param>
    private void UpdatePlayerClassText(string className)
    {
        if (_playerClassText != null)
            _playerClassText.text = string.IsNullOrWhiteSpace(className) ? "None" : className;
    }

    #endregion



    #region Логирование

    /// <summary>
    /// Метод записывает информационное сообщение в лог.
    /// </summary>
    private void LogInfo(string message)
    {
        Logger.Log(LogLevel.Info, nameof(PlayerCreationMenuView), message);
    }

    /// <summary>
    /// Метод записывает предупреждение в лог.
    /// </summary>
    private void LogWarning(string message)
    {
        Logger.Log(LogLevel.Warning, nameof(PlayerCreationMenuView), message);
    }

    /// <summary>
    /// Метод записывает ошибку в лог.
    /// </summary>
    private void LogError(string message)
    {
        Logger.Log(LogLevel.Error, nameof(PlayerCreationMenuView), message);
    }

    #endregion
}