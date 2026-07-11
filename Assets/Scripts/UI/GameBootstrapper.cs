using System.Collections;
using UnityEngine;

/// <summary>
/// Отвечает за первичную инициализацию игры.
/// Показывает экран загрузки, выполняет стартовые инициализации
/// и скрывает экран загрузки после завершения.
/// </summary>
public class GameBootstrapper : MonoBehaviour
{
    #region Инспектор

    /// <summary>
    /// Экран загрузки, который отображается во время старта игры.
    /// </summary>
    [SerializeField] private GameObject initialLoadingScreenObject;

    /// <summary>
    /// Менеджер меню настроек.
    /// Нужен для инициализации UI после загрузки настроек.
    /// </summary>
    [SerializeField] private SettingsMenuManager settingsMenuManager;

    /// <summary>
    /// Менеджер главного меню.
    /// Нужен для финальной подготовки интерфейса стартовой сцены.
    /// </summary>
    [SerializeField] private MainMenuManager mainMenuManager;

    /// <summary>
    /// Флаг, нужно ли загружать локализацию при старте игры.
    /// </summary>
    [SerializeField] private bool loadLocalizationOnStart = true;

    /// <summary>
    /// Флаг, нужно ли применять системные настройки при старте игры.
    /// </summary>
    [SerializeField] private bool applySystemSettingsOnStart = true;

    /// <summary>
    /// Флаг, нужно ли инициализировать главное меню при старте игры.
    /// </summary>
    [SerializeField] private bool initializeMainMenuOnStart = true;

    #endregion


    #region Unity

    /// <summary>
    /// Вызывается Unity один раз при создании объекта.
    /// Запускает асинхронную инициализацию игры.
    /// </summary>
    private void Awake()
    {
        StartCoroutine(InitializeGameRoutine());
    }

    #endregion


    #region Инициализация

    /// <summary>
    /// Основная корутина инициализации.
    /// Сначала показывает экран загрузки, затем выполняет все шаги старта.
    /// </summary>
    private IEnumerator InitializeGameRoutine()
    {
        // Показываем экран загрузки сразу при старте.
        ShowLoadingScreen();

        // Даём Unity один кадр, чтобы экран загрузки успел отрисоваться.
        yield return null;

        // 1. Загружаем настройки.
        SettingsService.Load();

        // 2. Настраиваем файловое логирование.
        SettingsService.ApplyFileLogging();

        // 3. Применяем системные настройки.
        if (applySystemSettingsOnStart)
        {
            SettingsService.ApplyToSystem();
        }

        // 4. Загружаем локализацию.
        if (loadLocalizationOnStart)
        {
            LocalizationService.LoadLocalization(SettingsService.LanguageCode);
        }

        // 5. Инициализируем UI настроек.
        if (settingsMenuManager != null)
        {
            settingsMenuManager.Initialize();
        }
        else
        {
            LogWarning("SettingsMenuManager is not assigned.");
        }

        // 6. Инициализируем главное меню.
        if (initializeMainMenuOnStart && mainMenuManager != null)
        {
            mainMenuManager.InitializeMenu();
        }
        else if (initializeMainMenuOnStart)
        {
            LogWarning("MainMenuManager is not assigned.");
        }

        // 7. Скрываем экран загрузки после завершения всех инициализаций.
        HideLoadingScreen();

        LogInfo("Game initialization completed.");
    }

    /// <summary>
    /// Показывает экран загрузки.
    /// </summary>
    private void ShowLoadingScreen()
    {
        if (initialLoadingScreenObject != null)
        {
            initialLoadingScreenObject.SetActive(true);
        }
    }

    /// <summary>
    /// Скрывает экран загрузки.
    /// </summary>
    private void HideLoadingScreen()
    {
        if (initialLoadingScreenObject != null)
        {
            initialLoadingScreenObject.SetActive(false);
        }
    }

    #endregion

    #region Логирование

    /// <summary>
    /// Записывает информационное сообщение.
    /// </summary>
    private void LogInfo(string message)
    {
        Logger.Log(LogLevel.Info, nameof(GameBootstrapper), message);
    }

    /// <summary>
    /// Записывает предупреждение.
    /// </summary>
    private void LogWarning(string message)
    {
        Logger.Log(LogLevel.Warning, nameof(GameBootstrapper), message);
    }

    #endregion
}