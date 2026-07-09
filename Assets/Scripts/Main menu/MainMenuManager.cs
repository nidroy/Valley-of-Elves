using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс управляет главным меню игры.
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    #region Приватные поля

    // Менеджер загрузки сцен.
    [SerializeField]
    private SceneLoader _sceneLoader;

    // Менеджер меню настроек.
    [SerializeField]
    private SettingsMenuManager _settingsMenuManager;

    // Объект меню настроек.
    [SerializeField]
    private GameObject _settingsMenuObject;

    // Кнопка продолжения игры.
    [SerializeField]
    private Button _continueButton;

    // Кнопка открытия меню настроек.
    [SerializeField]
    private Button _settingsButton;

    // Название сцены игры.
    [SerializeField]
    private string _defaultGameScene = "Game";

    #endregion


    #region Unity методы

    /// <summary>
    /// Метод вызывается при запуске объекта.
    /// </summary>
    private void Start()
    {
        try
        {
            // Показываем экран загрузки.
            ShowLoadingScreen();

            // Скрываем меню настроек.
            HideSettingsMenu();

            // Инициализируем главное меню.
            InitializeMenu();

            // Обновляем состояние кнопок.
            UpdateMenuButtons();

            // Скрываем экран загрузки.
            HideLoadingScreen();

            LogInfo("Main menu initialized successfully.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to initialize main menu: {exception.Message}");
        }
    }

    #endregion


    #region Публичные методы

    /// <summary>
    /// Метод запускает новую игру.
    /// </summary>
    public void NewGame()
    {
        try
        {
            // Создаем новое сохранение.
            GameData.CreateNewSave();

            // Проверяем сцену сохранения.
            PrepareGameScene();

            // Загружаем игровую сцену.
            LoadGameScene();

            LogInfo("New game started.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to start new game: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод продолжает сохраненную игру.
    /// </summary>
    public void ContinueGame()
    {
        try
        {
            // Проверяем наличие сохранения.
            if (!GameData.SaveFileExists())
            {
                LogWarning("Cannot continue game. Save file does not exist.");

                return;
            }

            // Загружаем сохраненные данные.
            if (!GameData.LoadSave())
            {
                LogError("Failed to load saved game.");

                return;
            }

            // Загружаем сцену сохранения.
            LoadGameScene();

            LogInfo("Saved game continued.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to continue game: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод показывает меню настроек.
    /// </summary>
    public void ShowSettingsMenu()
    {
        try
        {
            // Проверяем менеджер настроек.
            if (_settingsMenuManager == null)
            {
                LogError("Settings menu manager is null.");

                return;
            }

            // Подготавливаем настройки.
            _settingsMenuManager.PrepareSettingsMenu();

            // Показываем меню настроек.
            ShowObject(_settingsMenuObject);

            LogInfo("Settings menu opened.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to open settings menu: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод скрывает меню настроек.
    /// </summary>
    public void HideSettingsMenu()
    {
        try
        {
            // Скрываем меню настроек.
            HideObject(_settingsMenuObject);

            LogInfo("Settings menu closed.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to close settings menu: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод закрывает игру.
    /// </summary>
    public void QuitGame()
    {
        try
        {
            // Записываем информацию о выходе.
            LogInfo("Application quit requested.");

            // Закрываем приложение.
            CloseApplication();
        }
        catch (Exception exception)
        {
            LogError($"Failed to quit application: {exception.Message}");
        }
    }

    #endregion


    #region Инициализация

    /// <summary>
    /// Метод выполняет первоначальную инициализацию меню.
    /// </summary>
    private void InitializeMenu()
    {
        // Инициализируем настройки.
        InitializeSettings();
    }

    /// <summary>
    /// Метод инициализирует систему настроек.
    /// </summary>
    private void InitializeSettings()
    {
        // Проверяем менеджер настроек.
        if (_settingsMenuManager == null)
        {
            LogError("Settings menu manager is null.");

            return;
        }

        // Загружаем и применяем настройки.
        _settingsMenuManager.Init();

        LogInfo("Settings initialized.");
    }

    #endregion


    #region Работа с игрой

    /// <summary>
    /// Метод подготавливает сцену игры.
    /// </summary>
    private void PrepareGameScene()
    {
        // Проверяем наличие названия сцены.
        if (!string.IsNullOrWhiteSpace(_defaultGameScene))
        {
            GameData.SceneName = _defaultGameScene;
        }
    }

    /// <summary>
    /// Метод загружает игровую сцену.
    /// </summary>
    private void LoadGameScene()
    {
        // Проверяем наличие загрузчика сцен.
        if (_sceneLoader == null)
        {
            LogError("Scene loader is null.");

            return;
        }

        // Загружаем сцену.
        _sceneLoader.LoadScene(GameData.SceneName);
    }

    /// <summary>
    /// Метод обновляет состояние кнопок главного меню.
    /// </summary>
    private void UpdateMenuButtons()
    {
        // Проверяем кнопку продолжения игры.
        UpdateContinueButton();

        // Проверяем кнопку настроек.
        UpdateSettingsButton();
    }

    /// <summary>
    /// Метод обновляет состояние кнопки продолжения игры.
    /// </summary>
    private void UpdateContinueButton()
    {
        // Проверяем наличие кнопки.
        if (_continueButton == null)
        {
            LogWarning("Continue button is null.");

            return;
        }

        // Активируем кнопку только при наличии сохранения.
        _continueButton.interactable = GameData.SaveFileExists();
    }

    /// <summary>
    /// Метод обновляет состояние кнопки настроек.
    /// </summary>
    private void UpdateSettingsButton()
    {
        // Проверяем наличие кнопки.
        if (_settingsButton == null)
        {
            LogWarning("Settings button is null.");

            return;
        }

        /// Проверяем возможность открытия меню настроек.
        _settingsButton.interactable =
            _settingsMenuObject != null &&
            _settingsMenuManager != null &&
            Settings.SettingsFileExists();
    }

    #endregion


    #region Экран загрузки

    /// <summary>
    /// Метод показывает экран загрузки.
    /// </summary>
    private void ShowLoadingScreen()
    {
        // Проверяем загрузчик сцен.
        if (_sceneLoader == null)
        {
            LogWarning("Scene loader is null.");

            return;
        }

        // Показываем экран загрузки.
        _sceneLoader.ShowLoadingScreen();
    }

    /// <summary>
    /// Метод скрывает экран загрузки.
    /// </summary>
    private void HideLoadingScreen()
    {
        // Проверяем загрузчик сцен.
        if (_sceneLoader == null)
        {
            LogWarning("Scene loader is null.");

            return;
        }

        // Скрываем экран загрузки.
        _sceneLoader.HideLoadingScreen();
    }

    #endregion


    #region Управление объектами

    /// <summary>
    /// Метод показывает объект интерфейса.
    /// </summary>
    /// <param name="gameObject">Объект для отображения.</param>
    private void ShowObject(GameObject gameObject)
    {
        // Проверяем объект.
        if (gameObject == null)
        {
            LogWarning("Cannot show null object.");

            return;
        }

        // Активируем объект.
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Метод скрывает объект интерфейса.
    /// </summary>
    /// <param name="gameObject">Объект для скрытия.</param>
    private void HideObject(GameObject gameObject)
    {
        // Проверяем объект.
        if (gameObject == null)
        {
            LogWarning("Cannot hide null object.");

            return;
        }

        // Отключаем объект.
        gameObject.SetActive(false);
    }

    #endregion


    #region Завершение приложения

    /// <summary>
    /// Метод закрывает приложение.
    /// </summary>
    private void CloseApplication()
    {
        // Завершаем приложение.
        Application.Quit();

#if UNITY_EDITOR

        // Останавливаем запуск игры в редакторе Unity.
        UnityEditor.EditorApplication.isPlaying = false;

#endif
    }

    #endregion


    #region Логирование

    /// <summary>
    /// Метод записывает информационное сообщение.
    /// </summary>
    /// <param name="message">Сообщение для записи.</param>
    private void LogInfo(string message)
    {
        Logger.Log(Logger.LogLevel.Info, nameof(MainMenuManager), message);
    }

    /// <summary>
    /// Метод записывает предупреждение.
    /// </summary>
    /// <param name="message">Сообщение предупреждения.</param>
    private void LogWarning(string message)
    {
        Logger.Log(Logger.LogLevel.Warning, nameof(MainMenuManager), message);
    }

    /// <summary>
    /// Метод записывает ошибку.
    /// </summary>
    /// <param name="message">Сообщение ошибки.</param>
    private void LogError(string message)
    {
        Logger.Log(Logger.LogLevel.Error, nameof(MainMenuManager), message);
    }

    #endregion
}