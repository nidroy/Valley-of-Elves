using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Отвечает только за загрузку игровых сцен и экран загрузки.
/// Не занимается UI меню и сохранением.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    #region Инспектор

    /// <summary>
    /// Объект экрана загрузки.
    /// </summary>
    [SerializeField] private GameObject loadingScreenObject;

    #endregion


    #region Поля состояния

    /// <summary>
    /// Флаг того, что сцена уже загружается.
    /// </summary>
    private bool _isLoading;

    #endregion


    #region Публичные методы

    /// <summary>
    /// Запускает загрузку указанной сцены.
    /// </summary>
    /// <param name="sceneName">Имя сцены.</param>
    public void LoadScene(string sceneName)
    {
        if (_isLoading)
        {
            LogWarning("Scene loading already in progress.");
            return;
        }

        if (string.IsNullOrWhiteSpace(sceneName))
        {
            LogError("Scene name is empty.");
            return;
        }

        if (!IsSceneInBuildSettings(sceneName))
        {
            LogError($"Scene not found in build settings: {sceneName}");
            return;
        }

        StartCoroutine(LoadSceneAsync(sceneName));
    }

    /// <summary>
    /// Показывает экран загрузки.
    /// </summary>
    public void ShowLoadingScreen()
    {
        if (loadingScreenObject != null)
        {
            loadingScreenObject.SetActive(true);
        }
    }

    /// <summary>
    /// Скрывает экран загрузки.
    /// </summary>
    public void HideLoadingScreen()
    {
        if (loadingScreenObject != null)
        {
            loadingScreenObject.SetActive(false);
        }
    }

    #endregion


    #region Загрузка сцены

    /// <summary>
    /// Асинхронно загружает сцену.
    /// </summary>
    /// <param name="sceneName">Имя сцены.</param>
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        _isLoading = true;
        Globals.IsSceneLoading = true;
        Globals.LoadingProgress = 0f;

        try
        {
            ShowLoadingScreen();

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            if (operation == null)
            {
                LogError($"Failed to start loading scene: {sceneName}");
                yield break;
            }

            operation.allowSceneActivation = true;

            while (!operation.isDone)
            {
                Globals.LoadingProgress = Mathf.Clamp01(operation.progress / 0.9f);
                yield return null;
            }

            LogInfo($"Scene loaded: {sceneName}");
        }
        finally
        {
            HideLoadingScreen();
            Globals.IsSceneLoading = false;
            Globals.LoadingProgress = 0f;
            _isLoading = false;
        }
    }

    #endregion


    #region Проверка сцены

    /// <summary>
    /// Проверяет, добавлена ли сцена в Build Settings.
    /// </summary>
    /// <param name="sceneName">Имя сцены.</param>
    /// <returns>True, если сцена найдена.</returns>
    private bool IsSceneInBuildSettings(string sceneName)
    {
        for (int index = 0; index < SceneManager.sceneCountInBuildSettings; index++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(index);
            string name = Path.GetFileNameWithoutExtension(path);

            if (name == sceneName)
            {
                return true;
            }
        }

        return false;
    }

    #endregion


    #region Логирование

    /// <summary>
    /// Записывает информационное сообщение в лог.
    /// </summary>
    private void LogInfo(string message)
    {
        Logger.Log(LogLevel.Info, nameof(SceneLoader), message);
    }

    /// <summary>
    /// Записывает предупреждение в лог.
    /// </summary>
    private void LogWarning(string message)
    {
        Logger.Log(LogLevel.Warning, nameof(SceneLoader), message);
    }

    /// <summary>
    /// Записывает ошибку в лог.
    /// </summary>
    private void LogError(string message)
    {
        Logger.Log(LogLevel.Error, nameof(SceneLoader), message);
    }

    #endregion
}
