using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Управляет главным меню игры.
/// Кнопки хранятся с ключами, чтобы не держать отдельное поле под Continue.
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    #region Внутренние типы

    /// <summary>
    /// Связка ключа и кнопки меню.
    /// Используется для поиска кнопок по имени вместо отдельных ссылок.
    /// </summary>
    [Serializable]
    public class MenuButtonBinding
    {
        /// <summary>
        /// Ключ кнопки, например: Continue, NewGame, Settings, Quit.
        /// </summary>
        public string Key;

        /// <summary>
        /// Ссылка на UI-кнопку.
        /// </summary>
        public Button Button;
    }

    #endregion


    #region Инспектор

    /// <summary>
    /// Ссылка на загрузчик сцен.
    /// </summary>
    [SerializeField] private SceneLoader sceneLoader;

    /// <summary>
    /// Ссылка на менеджер меню настроек.
    /// </summary>
    [SerializeField] private SettingsMenuManager settingsMenuManager;

    /// <summary>
    /// Корневой объект панели настроек.
    /// </summary>
    [SerializeField] private GameObject settingsMenuObject;

    /// <summary>
    /// Имя игровой сцены.
    /// </summary>
    [SerializeField] private string gameSceneName = "Game";

    /// <summary>
    /// Все кнопки меню с ключами.
    /// </summary>
    [SerializeField] private MenuButtonBinding[] menuButtons;

    #endregion


    #region Константы

    /// <summary>
    /// Ключ кнопки продолжения игры.
    /// </summary>
    private const string ContinueButtonKey = "Continue";

    #endregion


    #region Инициализация

    /// <summary>
    /// Инициализирует главное меню.
    /// Вызывается bootstrapper-ом после старта игры.
    /// </summary>
    public void InitializeMenu()
    {
        ValidateDependencies();
        HideSettingsMenu();

        if (settingsMenuManager != null)
        {
            settingsMenuManager.Initialize();
        }

        SetMenuInteractable(true);
        UpdateContinueButton();
    }

    #endregion


    #region Публичные методы

    /// <summary>
    /// Начинает новую игру.
    /// </summary>
    public void NewGame()
    {
        try
        {
            SetMenuInteractable(false);

            SaveGameService.CreateNew();
            SaveGameService.SceneName = gameSceneName;

            LoadGameScene();

            LogInfo("New game started.");

            UpdateContinueButton();
        }
        catch (Exception exception)
        {
            SetMenuInteractable(true);
            LogError($"Failed to start new game: {exception.Message}");
        }
    }

    /// <summary>
    /// Продолжает игру из сохранения.
    /// </summary>
    public void ContinueGame()
    {
        try
        {
            if (!SaveGameService.Load())
            {
                LogWarning("Failed to load save.");
                UpdateContinueButton();
                return;
            }

            SetMenuInteractable(false);
            LoadGameScene();

            LogInfo("Game continued.");
        }
        catch (Exception exception)
        {
            SetMenuInteractable(true);
            LogError($"Failed to continue game: {exception.Message}");
        }
    }

    /// <summary>
    /// Открывает меню настроек.
    /// </summary>
    public void ShowSettingsMenu()
    {
        if (settingsMenuManager == null)
        {
            LogError("Settings manager is null.");
            return;
        }

        settingsMenuManager.RefreshUI();
        SetObjectActive(settingsMenuObject, true);
    }

    /// <summary>
    /// Закрывает меню настроек.
    /// </summary>
    public void HideSettingsMenu()
    {
        SetObjectActive(settingsMenuObject, false);
    }

    /// <summary>
    /// Завершает приложение.
    /// </summary>
    public void QuitGame()
    {
        LogInfo("Application quit requested.");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    #endregion


    #region Инициализация меню

    /// <summary>
    /// Проверяет зависимости и выполняет стартовую настройку меню.
    /// </summary>
    private void ValidateDependencies()
    {
        if (sceneLoader == null)
        {
            LogError("Scene loader is missing.");
        }

        if (settingsMenuObject == null)
        {
            LogWarning("Settings menu object is missing.");
        }
    }

    /// <summary>
    /// Загружает игровую сцену через SceneLoader.
    /// </summary>
    private void LoadGameScene()
    {
        if (sceneLoader == null)
        {
            LogError("Scene loader is null.");
            return;
        }

        sceneLoader.LoadScene(SaveGameService.SceneName);
    }

    /// <summary>
    /// Делает кнопку Continue активной только если файл сохранения существует.
    /// </summary>
    private void UpdateContinueButton()
    {
        Button continueButton = FindButtonByKey(ContinueButtonKey);

        if (continueButton != null)
        {
            continueButton.interactable = SaveGameService.HasSave;
        }
        else
        {
            LogWarning($"Button with key '{ContinueButtonKey}' not found.");
        }
    }

    #endregion


    #region Работа с кнопками

    /// <summary>
    /// Ищет кнопку по ключу в массиве menuButtons.
    /// </summary>
    /// <param name="key">Ключ кнопки.</param>
    /// <returns>Найденная кнопка или null.</returns>
    private Button FindButtonByKey(string key)
    {
        if (menuButtons == null || string.IsNullOrWhiteSpace(key))
        {
            return null;
        }

        foreach (MenuButtonBinding binding in menuButtons)
        {
            if (binding == null || binding.Button == null)
            {
                continue;
            }

            if (binding.Key == key)
            {
                return binding.Button;
            }
        }

        return null;
    }

    /// <summary>
    /// Изменяет интерактивность всех кнопок меню.
    /// </summary>
    /// <param name="state">Новое состояние.</param>
    private void SetMenuInteractable(bool state)
    {
        if (menuButtons == null)
        {
            return;
        }

        foreach (MenuButtonBinding binding in menuButtons)
        {
            if (binding != null && binding.Button != null)
            {
                binding.Button.interactable = state;
            }
        }
    }

    #endregion


    #region Работа с объектами

    /// <summary>
    /// Включает или выключает указанный объект.
    /// </summary>
    /// <param name="target">Целевой объект.</param>
    /// <param name="state">Нужно ли сделать объект активным.</param>
    private void SetObjectActive(GameObject target, bool state)
    {
        if (target != null)
        {
            target.SetActive(state);
        }
    }

    #endregion


    #region Логирование

    /// <summary>
    /// Записывает информационное сообщение.
    /// </summary>
    private void LogInfo(string message)
    {
        Logger.Log(LogLevel.Info, nameof(MainMenuManager), message);
    }

    /// <summary>
    /// Записывает предупреждение.
    /// </summary>
    private void LogWarning(string message)
    {
        Logger.Log(LogLevel.Warning, nameof(MainMenuManager), message);
    }

    /// <summary>
    /// Записывает ошибку.
    /// </summary>
    private void LogError(string message)
    {
        Logger.Log(LogLevel.Error, nameof(MainMenuManager), message);
    }

    #endregion
}