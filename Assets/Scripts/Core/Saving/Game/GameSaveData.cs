using System;

/// <summary>
/// Модель данных сохранения игры.
/// </summary>
[Serializable]
public class GameSaveData
{
    /// <summary>
    /// Версия структуры сохранения игры.
    /// </summary>
    public int Version = Globals.GameSaveDefaultVersion;

    /// <summary>
    /// Название сцены, в которой находится игрок или в которую нужно загрузить его при восстановлении.
    /// </summary>
    public string SceneName = Globals.GameSaveDefaultSceneName;
}
