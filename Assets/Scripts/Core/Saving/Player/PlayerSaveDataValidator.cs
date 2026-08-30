using UnityEngine;

/// <summary>
/// Валидатор данных сохранения игрока.
/// Нормализатор приводит значения к безопасным диапазонам.
/// </summary>
public static class PlayerSaveDataValidator
{
    /// <summary>
    /// Метод приводит данные сохранения игрока к безопасному и корректному состоянию.
    /// </summary>
    /// <param name="data">Объект данных сохранения игрока, который нужно проверить и исправить.</param>
    public static void Normalize(PlayerSaveData data)
    {
        // Если объект не передан, ничего не делаем.
        if (data == null)
        {
            return;
        }



        // Версия не может быть меньше 1.
        data.Version = Mathf.Max(1, data.Version);

        // Уровень не может быть меньше 1.
        data.Level = Mathf.Max(1, data.Level);

        // Опыт не может быть отрицательным.
        data.Experience = Mathf.Max(0, data.Experience);

        // Максимальное здоровье не может быть меньше 1.
        data.MaxHealth = Mathf.Max(1, data.MaxHealth);

        // Текущее здоровье должно находиться в диапазоне от 0 до MaxHealth.
        data.Health = Mathf.Clamp(data.Health, 0, data.MaxHealth);

        // Максимальная мана не может быть отрицательной.
        data.MaxMana = Mathf.Max(0, data.MaxMana);

        // Текущая мана должна находиться в диапазоне от 0 до MaxMana.
        data.Mana = Mathf.Clamp(data.Mana, 0, data.MaxMana);

        // Золото не может быть отрицательным.
        data.Gold = Mathf.Max(0, data.Gold);



        // Имя игрока должно быть задано, если оно пустое.
        if (string.IsNullOrWhiteSpace(data.Name))
        {
            data.Name = Globals.PlayerSaveDefaultName;
        }
        else
        {
            data.Name = data.Name.Trim();
        }

        // Класс игрока должен быть задан, если он пустой.
        if (string.IsNullOrWhiteSpace(data.Class))
        {
            data.Class = Globals.PlayerSaveDefaultClass;
        }
        else
        {
            data.Class = data.Class.Trim();
        }

        // Текст класса игрока должен быть задан, если он пустой.
        if (string.IsNullOrWhiteSpace(data.ClassText))
        {
            data.ClassText = Globals.PlayerSaveDefaultClassText;
        }
        else
        {
            data.ClassText = data.ClassText.Trim();
        }
    }
}