using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Класс управляет загрузкой игровых сцен и экраном загрузки.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    #region Приватные поля

    // Объект экрана загрузки.
    [SerializeField]
    private GameObject _loadingScreenObject;

    #endregion


    #region Публичные методы

    /// <summary>
    /// Метод загружает указанную сцену.
    /// </summary>
    /// <param name="sceneName">Название сцены для загрузки.</param>
    public void LoadScene(string sceneName)
    {
        try
        {
            // Проверяем корректность имени сцены.
            if (!ValidateSceneName(sceneName))
            {
                LogError("Scene name cannot be empty.");

                return;
            }

            // Запускаем процесс загрузки сцены.
            StartCoroutine(LoadSceneAsync(sceneName));

            LogInfo($"Scene loading started: {sceneName}.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to load scene: {exception.Message}");
        }
    }

    #endregion


    #region Загрузка сцены

    /// <summary>
    /// Метод выполняет асинхронную загрузку сцены.
    /// </summary>
    /// <param name="sceneName">Название загружаемой сцены.</param>
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Устанавливаем состояние загрузки сцены.
        Globals.IsSceneLoading = true;

        // Показываем экран загрузки.
        ShowLoadingScreen();

        // Запускаем асинхронную загрузку сцены.
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Проверяем успешность запуска загрузки.
        if (operation == null)
        {
            LogError($"Failed to start loading scene: {sceneName}.");

            HideLoadingScreen();

            yield break;
        }

        // Ожидаем завершения загрузки.
        while (!operation.isDone)
        {
            yield return null;
        }

        // Скрываем экран загрузки.
        HideLoadingScreen();

        // Устанавливаем состояние завершения загрузки.
        Globals.IsSceneLoading = false;

        LogInfo($"Scene loaded successfully: {sceneName}.");
    }

    #endregion


    #region Экран загрузки

    /// <summary>
    /// Метод показывает экран загрузки.
    /// </summary>
    public void ShowLoadingScreen()
    {
        try
        {
            // Проверяем наличие экрана загрузки.
            if (_loadingScreenObject == null)
            {
                LogWarning("Loading screen object is null.");

                return;
            }

            // Активируем экран загрузки.
            _loadingScreenObject.SetActive(true);

            LogInfo("Loading screen shown.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to show loading screen: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод скрывает экран загрузки.
    /// </summary>
    public void HideLoadingScreen()
    {
        try
        {
            // Проверяем наличие экрана загрузки.
            if (_loadingScreenObject == null)
            {
                LogWarning("Loading screen object is null.");

                return;
            }

            // Отключаем экран загрузки.
            _loadingScreenObject.SetActive(false);

            LogInfo("Loading screen hidden.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to hide loading screen: {exception.Message}");
        }
    }

    #endregion


    #region Проверка данных

    /// <summary>
    /// Метод проверяет корректность имени сцены.
    /// </summary>
    /// <param name="sceneName">Имя сцены для проверки.</param>
    /// <returns>True, если имя сцены корректное.</returns>
    private bool ValidateSceneName(string sceneName)
    {
        // Проверяем наличие значения.
        return !string.IsNullOrWhiteSpace(sceneName);
    }

    #endregion


    #region Логирование

    /// <summary>
    /// Метод записывает информационное сообщение.
    /// </summary>
    /// <param name="message">Сообщение для записи.</param>
    private void LogInfo(string message)
    {
        Logger.Log(Logger.LogLevel.Info, nameof(SceneLoader), message);
    }

    /// <summary>
    /// Метод записывает предупреждение.
    /// </summary>
    /// <param name="message">Сообщение предупреждения.</param>
    private void LogWarning(string message)
    {
        Logger.Log(Logger.LogLevel.Warning, nameof(SceneLoader), message);
    }

    /// <summary>
    /// Метод записывает ошибку.
    /// </summary>
    /// <param name="message">Сообщение ошибки.</param>
    private void LogError(string message)
    {
        Logger.Log(Logger.LogLevel.Error, nameof(SceneLoader), message);
    }

    #endregion
}