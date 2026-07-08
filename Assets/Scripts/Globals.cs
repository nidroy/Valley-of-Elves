
/// <summary>
/// Класс хранит глобальные данные игры.
/// </summary>
public static class Globals
{
    #region Приватные поля

    // Состояние загрузки текущей сцены.
    private static bool _isSceneLoading = true;

    // Имя текущего игрока.
    private static string _playerName = string.Empty;

    #endregion


    #region Публичные свойства

    /// <summary>
    /// Свойство получает или устанавливает состояние загрузки сцены.
    /// </summary>
    public static bool IsSceneLoading
    {
        get => _isSceneLoading;

        set => _isSceneLoading = value;
    }

    /// <summary>
    /// Свойство получает или устанавливает имя игрока.
    /// </summary>
    public static string PlayerName
    {
        get => _playerName;

        set
        {
            if (ValidatePlayerName(value))
            {
                _playerName = value.Trim();
                return;
            }

            LogError("Player name cannot be null or empty.");
        }
    }

    #endregion


    #region Публичные методы

    /// <summary>
    /// Метод очищает сохраненное имя игрока.
    /// </summary>
    public static void ClearPlayerName()
    {
        // Устанавливаем пустое значение имени игрока.
        _playerName = string.Empty;

        LogInfo("Player name cleared.");
    }

    #endregion


    #region Приватные методы проверки

    /// <summary>
    /// Метод проверяет корректность имени игрока.
    /// </summary>
    /// <param name="playerName">Имя игрока для проверки.</param>
    /// <returns>True, если имя корректное.</returns>
    private static bool ValidatePlayerName(string playerName)
    {
        // Проверяем, что строка существует и содержит символы.
        return !string.IsNullOrWhiteSpace(playerName);
    }

    #endregion


    #region Логирование

    /// <summary>
    /// Метод записывает информационное сообщение.
    /// </summary>
    /// <param name="message">Сообщение для записи.</param>
    private static void LogInfo(string message)
    {
        Logger.Log(Logger.LogLevel.Info, nameof(Globals), message);
    }

    /// <summary>
    /// Метод записывает предупреждение.
    /// </summary>
    /// <param name="message">Сообщение предупреждения.</param>
    private static void LogWarning(string message)
    {
        Logger.Log(Logger.LogLevel.Warning, nameof(Globals), message);
    }

    /// <summary>
    /// Метод записывает ошибку.
    /// </summary>
    /// <param name="message">Сообщение ошибки.</param>
    private static void LogError(string message)
    {
        Logger.Log(Logger.LogLevel.Error, nameof(Globals), message);
    }

    #endregion
}