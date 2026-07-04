using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _loadingObject; // Объект, отображающий экран загрузки

    [SerializeField]
    private SettingsMenuManager _settingsMenuManager; // Ссылка на SettingsMenuManager

    // Свойство для доступа к объекту загрузки
    public GameObject LoadingObject
    {
        get => _loadingObject;
        private set => _loadingObject = value;
    }

    private void Start()
    {
        _settingsMenuManager.Init(); // Инициализация настроек
    }

    #region Публичные методы

    /// <summary>
    /// Обработчик нажатия кнопки для выхода из игры.
    /// </summary>
    public void OnQuitGameButtonClick()
    {
        LogInfo("Exiting game...");

        // Завершение приложения
        Application.Quit();

#if UNITY_EDITOR
        // Остановка игры в редакторе Unity
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    /// <summary>
    /// Метод для показа или скрытия экранов.
    /// </summary>
    /// <param name="screenToShow">Экран, который нужно показать</param>
    /// <param name="screenToHide">Экран, который нужно скрыть</param>
    public void ShowScreen(GameObject screenToShow = null, GameObject screenToHide = null)
    {
        if (screenToShow != null)
        {
            screenToShow.SetActive(true); // Активировать экран для показа
            LogInfo($"Showing screen: {screenToShow.name}.");
        }
        else
        {
            LogWarning("screenToShow is null, cannot show the screen!");
        }

        if (screenToHide != null)
        {
            screenToHide.SetActive(false); // Деактивировать экран для скрытия
            LogInfo($"Hiding screen: {screenToHide.name}.");
        }
        else
        {
            LogWarning("screenToHide is null, nothing to hide!");
        }
    }

    #endregion

    #region Приватные методы

    /// <summary>
    /// Метод для логирования предупреждений
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private void LogWarning(string message)
    {
        Logger.Log(Logger.LogLevel.Warning, nameof(MainMenuManager), message);
    }

    /// <summary>
    /// Метод для логирования информационных сообщений
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private void LogInfo(string message)
    {
        Logger.Log(Logger.LogLevel.Info, nameof(MainMenuManager), message);
    }

    #endregion
}
