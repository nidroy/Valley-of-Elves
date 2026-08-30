using UnityEngine;

/// <summary>
/// Валидатор данных сохранения игры.
/// Нормализатор приводит значения к безопасным диапазонам.
/// </summary>
public static class GameSaveDataValidator
{
    /// <summary>
    /// Метод приводит данные сохранения игры к безопасному и корректному состоянию.
    /// </summary>
    /// <param name="data">Объект данных сохранения игры, который нужно проверить и исправить.</param>
    public static void Normalize(GameSaveData data)
    {
        // Если объект не передан, ничего не делаем.
        if (data == null)
        {
            return;
        }



        // Версия не может быть меньше 1.
        data.Version = Mathf.Max(1, data.Version);



        // Название сцены должно быть задано.
        if (string.IsNullOrWhiteSpace(data.SceneName))
        {
            data.SceneName = Globals.GameSaveDefaultSceneName;
        }
        else
        {
            data.SceneName = data.SceneName.Trim();
        }
    }
}