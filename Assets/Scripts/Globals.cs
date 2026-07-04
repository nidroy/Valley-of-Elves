
public static class Globals
{
    // Значение состояния загрузки сцены
    private static bool _isSceneLoading = true;

    // Свойство для доступа к состоянию загрузки сцены
    public static bool IsSceneLoading
    {
        get => _isSceneLoading;
        set => _isSceneLoading = value;
    }

    // Имя игрока
    private static string _playerName = string.Empty;

    // Свойство для получения и установки имени игрока
    public static string PlayerName
    {
        get => _playerName;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                _playerName = value;
            }
            else
            {
                LogError("Player name cannot be null or empty!");
            }
        }
    }

    /// <summary>
    /// Метод для логирования ошибок
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private static void LogError(string message)
    {
        Logger.Log(Logger.LogLevel.Error, nameof(Globals), message);
    }
}
