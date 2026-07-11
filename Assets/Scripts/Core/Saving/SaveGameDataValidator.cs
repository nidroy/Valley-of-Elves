using UnityEngine;

/// <summary>
/// Отвечает только за проверку и нормализацию данных сохранения.
/// Не знает ничего о файлах, UI и сценах.
/// </summary>
public static class SaveGameDataValidator
{
    /// <summary>
    /// Приводит данные сохранения к безопасному и корректному состоянию.
    /// </summary>
    /// <param name="data">Данные сохранения.</param>
    public static void Normalize(SaveGameData data)
    {
        if (data == null)
        {
            return;
        }

        data.Version = Mathf.Max(1, data.Version);
        data.Level = Mathf.Max(1, data.Level);
        data.Experience = Mathf.Max(0, data.Experience);

        data.MaxHealth = Mathf.Max(1, data.MaxHealth);
        data.Health = Mathf.Clamp(data.Health, 0, data.MaxHealth);

        data.MaxMana = Mathf.Max(0, data.MaxMana);
        data.Mana = Mathf.Clamp(data.Mana, 0, data.MaxMana);

        data.Gold = Mathf.Max(0, data.Gold);

        if (string.IsNullOrWhiteSpace(data.SceneName))
        {
            data.SceneName = "Game";
        }

        if (string.IsNullOrWhiteSpace(data.PlayerName))
        {
            data.PlayerName = string.Empty;
        }

        if (string.IsNullOrWhiteSpace(data.PlayerClass))
        {
            data.PlayerClass = string.Empty;
        }
    }
}