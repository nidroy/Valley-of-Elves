using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Контроллер загрузчика сцен.
/// Отвечает за асинхронную загрузку сцен и отображение экрана загрузки.
/// </summary>
public class SceneLoaderController
{
    #region Поля

    /// <summary>
    /// Объект, который может запускать корутины.
    /// </summary>
    private readonly MonoBehaviour _coroutineRunner;

    /// <summary>
    /// Экран загрузки.
    /// </summary>
    private readonly GameObject _loadingScreen;

    /// <summary>
    /// Минимальное время загрузки в секундах.
    /// </summary>
    private readonly float _minimumLoadingTime;

    /// <summary>
    /// Флаг, предотвращающий повторный запуск загрузки.
    /// </summary>
    private bool _isLoading;

    #endregion



    #region Конструктор

    /// <summary>
    /// Конструктор создаёт новый контроллер загрузчика сцен.
    /// </summary>
    /// <param name="coroutineRunner">Объект, запускающий корутины.</param>
    /// <param name="loadingScreen">Экран загрузки. Может быть null.</param>
    /// <param name="minimumLoadingTime">Минимальное время загрузки в секундах.</param>
    public SceneLoaderController(MonoBehaviour coroutineRunner, GameObject loadingScreen = null, float minimumLoadingTime = 1f)
    {
        _coroutineRunner = coroutineRunner;
        _loadingScreen = loadingScreen;
        _minimumLoadingTime = Mathf.Max(0f, minimumLoadingTime);

        LogInfo($"Scene loader controller created. Loading screen assigned: {_loadingScreen != null}, minimum time: {_minimumLoadingTime:0.00}s.");
    }

    #endregion



    #region Публичные методы

    /// <summary>
    /// Метод запускает асинхронную загрузку сцены.
    /// </summary>
    /// <param name="sceneName">Название загружаемой сцены.</param>
    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            LogWarning("Scene name is empty. Scene loading aborted.");
            return;
        }

        if (_coroutineRunner == null)
        {
            LogError("Coroutine runner is not assigned. Scene loading aborted.");
            return;
        }

        if (_isLoading)
        {
            LogWarning("Scene loading is already in progress.");
            return;
        }

        _isLoading = true;
        _coroutineRunner.StartCoroutine(LoadSceneRoutine(sceneName));
    }

    #endregion



    #region Основная логика

    /// <summary>
    /// Метод выполняет асинхронную загрузку сцены.
    /// </summary>
    /// <param name="sceneName">Название загружаемой сцены.</param>
    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        LogInfo($"Scene loading started: {sceneName}");

        ShowLoadingScreen();

        float startTime = Time.realtimeSinceStartup;
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);

        if (loadOperation == null)
        {
            LogError($"Failed to start async loading for scene: {sceneName}");

            _isLoading = false;
            HideLoadingScreen();

            yield break;
        }

        loadOperation.allowSceneActivation = false;

        LogInfo($"Async loading started for scene: {sceneName}");

        while (loadOperation.progress < 0.9f)
        {
            yield return null;
        }

        float elapsedTime = Time.realtimeSinceStartup - startTime;
        float remainingTime = _minimumLoadingTime - elapsedTime;

        if (remainingTime > 0f)
        {
            LogInfo($"Waiting for minimum loading time: {remainingTime:0.00}s.");
            yield return new WaitForSecondsRealtime(remainingTime);
        }

        loadOperation.allowSceneActivation = true;

        while (!loadOperation.isDone)
        {
            yield return null;
        }

        LogInfo($"Scene loaded successfully: {sceneName}");

        HideLoadingScreen();

        _isLoading = false;
    }

    #endregion



    #region Экран загрузки

    /// <summary>
    /// Метод отображает экран загрузки.
    /// </summary>
    private void ShowLoadingScreen()
    {
        if (_loadingScreen == null)
        {
            LogWarning("Loading screen is not assigned.");
            return;
        }

        _loadingScreen.SetActive(true);

        LogInfo("Loading screen shown.");
    }

    /// <summary>
    /// Метод скрывает экран загрузки.
    /// </summary>
    private void HideLoadingScreen()
    {
        if (_loadingScreen == null)
        {
            LogWarning("Loading screen is not assigned.");
            return;
        }

        _loadingScreen.SetActive(false);

        LogInfo("Loading screen hidden.");
    }

    #endregion



    #region Логирование

    /// <summary>
    /// Метод записывает информационное сообщение в лог.
    /// </summary>
    private void LogInfo(string message)
    {
        Logger.Log(LogLevel.Info, nameof(SceneLoaderController), message);
    }

    /// <summary>
    /// Метод записывает предупреждение в лог.
    /// </summary>
    private void LogWarning(string message)
    {
        Logger.Log(LogLevel.Warning, nameof(SceneLoaderController), message);
    }

    /// <summary>
    /// Метод записывает ошибку в лог.
    /// </summary>
    private void LogError(string message)
    {
        Logger.Log(LogLevel.Error, nameof(SceneLoaderController), message);
    }

    #endregion
}