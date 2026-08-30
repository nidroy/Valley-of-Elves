using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Представление стартовой сцены.
/// Выполняет первичную инициализацию основных систем игры.
/// </summary>
public class StartupSceneView : MonoBehaviour
{
    /// <summary>
    /// Название сцены главного меню.
    /// </summary>
    [SerializeField] private string _mainMenuSceneName = "Main Menu Scene";

    /// <summary>
    /// Минимальное время отображения стартовой сцены в секундах.
    /// </summary>
    [SerializeField] private float _minimumLoadingTime = 1f;



    /// <summary>
    /// Флаг, предотвращающий повторную инициализацию.
    /// </summary>
    private static bool _isInitialized;



    /// <summary>
    /// Метод Unity запускает процесс инициализации.
    /// </summary>
    private void Start()
    {
        StartCoroutine(StartInitialization());
    }



    /// <summary>
    /// Метод основной инициализации.
    /// </summary>
    private IEnumerator StartInitialization()
    {
        if (_isInitialized)
        {
            LogInfo("Startup scene already initialized. Loading main menu.");
            yield return LoadMainMenu();
            yield break;
        }

        _isInitialized = true;

        LogInfo("Startup scene initialization started.");

        float startTime = Time.realtimeSinceStartup;

        InitializeSettings();
        InitializeLocalization();
        InitializePlayerSave();
        InitializeGameSave();

        float elapsedTime = Time.realtimeSinceStartup - startTime;
        float remainingTime = _minimumLoadingTime - elapsedTime;

        if (remainingTime > 0f)
        {
            yield return new WaitForSeconds(remainingTime);
        }

        LogInfo("Startup scene initialization finished.");

        yield return LoadMainMenu();
    }

    /// <summary>
    /// Метод инициализирует настройки игры.
    /// </summary>
    private void InitializeSettings()
    {
        try
        {
            if (SettingsService.Load())
            {
                LogInfo("Settings loaded successfully.");
            }
            else
            {
                LogWarning("Settings loading failed. Default settings will be applied.");
                SettingsService.Default();
                SettingsService.Save();
            }

            SettingsService.Apply();
            LogInfo("Settings applied successfully.");
        }
        catch (Exception exception)
        {
            LogError($"Settings initialization failed: {exception.Message}");
            SettingsService.Default();
            SettingsService.Apply();
        }
    }

    /// <summary>
    /// Метод инициализирует локализацию.
    /// </summary>
    private void InitializeLocalization()
    {
        try
        {
            string languageCode = SettingsService.LanguageCode;

            if (LocalizationService.LoadLocalization(languageCode))
            {
                LogInfo($"Localization loaded successfully. Language: {languageCode}");
            }
            else
            {
                LogWarning($"Localization loading failed for language '{languageCode}'.");
            }
        }
        catch (Exception exception)
        {
            LogError($"Localization initialization failed: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод инициализирует данные сохранения игрока.
    /// </summary>
    private void InitializePlayerSave()
    {
        try
        {
            if (PlayerSaveService.Load())
            {
                LogInfo("Player save loaded successfully.");
            }
            else
            {
                LogWarning("Player save loading failed. Default save data will be used.");
            }
        }
        catch (Exception exception)
        {
            LogError($"Player save initialization failed: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод инициализирует данные сохранения игры.
    /// </summary>
    private void InitializeGameSave()
    {
        try
        {
            if (GameSaveService.Load())
            {
                LogInfo("Game save loaded successfully.");
            }
            else
            {
                LogWarning("Game save loading failed. Default save data will be used.");
            }
        }
        catch (Exception exception)
        {
            LogError($"Game save initialization failed: {exception.Message}");
        }
    }



    /// <summary>
    /// Метод выполняет переход в главное меню.
    /// </summary>
    private IEnumerator LoadMainMenu()
    {
        yield return null;

        try
        {
            LogInfo($"Loading main menu scene: {_mainMenuSceneName}");
            SceneManager.LoadScene(_mainMenuSceneName);
        }
        catch (Exception exception)
        {
            LogError($"Failed to load main menu scene: {exception.Message}");
        }
    }



    /// <summary>
    /// Метод записывает информационное сообщение в лог.
    /// </summary>
    private void LogInfo(string message)
    {
        Logger.Log(LogLevel.Info, nameof(StartupSceneView), message);
    }

    /// <summary>
    /// Метод записывает предупреждение в лог.
    /// </summary>
    private void LogWarning(string message)
    {
        Logger.Log(LogLevel.Warning, nameof(StartupSceneView), message);
    }

    /// <summary>
    /// Метод записывает ошибку в лог.
    /// </summary>
    private void LogError(string message)
    {
        Logger.Log(LogLevel.Error, nameof(StartupSceneView), message);
    }
}