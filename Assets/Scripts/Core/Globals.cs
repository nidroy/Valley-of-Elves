/// <summary>
/// Глобальные флаги и значения, используемые разными системами игры.
/// </summary>
public static class Globals
{
    /// <summary>
    /// True, если в данный момент выполняется загрузка сцены.
    /// </summary>
    public static bool IsSceneLoading { get; set; }

    /// <summary>
    /// Прогресс загрузки сцены в диапазоне 0..1.
    /// </summary>
    public static float LoadingProgress { get; set; }
}