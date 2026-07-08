using System;
using UnityEngine;

/// <summary>
/// Класс управляет главным меню игры.
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    #region Приватные поля

    // Экран загрузки игры.
    [SerializeField]
    private GameObject _loadingScreenObject;

    // Экран меню настроек.
    [SerializeField]
    private GameObject _settingsMenuObject;

    // Менеджер меню настроек.
    [SerializeField]
    private SettingsMenuManager _settingsMenuManager;

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
            if (!InitializeMenu())
            {
                LogError("Main menu initialization failed.");

                return;
            }

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
    /// <returns>True, если инициализация успешна.</returns>
    private bool InitializeMenu()
    {
        // Инициализируем настройки.
        return InitializeSettings();
    }

    /// <summary>
    /// Метод инициализирует систему настроек.
    /// </summary>
    /// <returns>True, если настройки успешно загружены.</returns>
    private bool InitializeSettings()
    {
        // Проверяем наличие менеджера настроек.
        if (_settingsMenuManager == null)
        {
            LogError("Settings menu manager is null.");

            return false;
        }

        // Загружаем и применяем настройки.
        _settingsMenuManager.Init();

        return true;
    }

    #endregion


    #region Управление экраном загрузки

    /// <summary>
    /// Метод показывает экран загрузки.
    /// </summary>
    private void ShowLoadingScreen()
    {
        // Показываем экран загрузки.
        ShowObject(_loadingScreenObject);


        LogInfo("Loading screen shown.");
    }

    /// <summary>
    /// Метод скрывает экран загрузки.
    /// </summary>
    private void HideLoadingScreen()
    {
        // Скрываем экран загрузки.
        HideObject(_loadingScreenObject);

        LogInfo("Loading screen hidden.");
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

        LogInfo($"Object shown: {gameObject.name}.");
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

        LogInfo($"Object hidden: {gameObject.name}.");
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

        // Останавливаем игру в редакторе Unity.
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