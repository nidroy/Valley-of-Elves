using System;

/// <summary>
/// Модель данных сохранения игры.
/// Содержит только данные, без логики файловой работы и без валидации.
/// </summary>
[Serializable]
public class SaveGameData
{
    /// <summary>
    /// Версия структуры сохранения.
    /// Используется для будущей миграции формата.
    /// </summary>
    public int Version = 1;

    /// <summary>
    /// Имя игрока.
    /// </summary>
    public string PlayerName = string.Empty;

    /// <summary>
    /// Класс/роль игрока.
    /// </summary>
    public string PlayerClass = string.Empty;

    /// <summary>
    /// Уровень персонажа.
    /// </summary>
    public int Level = 1;

    /// <summary>
    /// Количество опыта.
    /// </summary>
    public int Experience = 0;

    /// <summary>
    /// Текущее здоровье.
    /// </summary>
    public int Health = 100;

    /// <summary>
    /// Максимальное здоровье.
    /// </summary>
    public int MaxHealth = 100;

    /// <summary>
    /// Текущая мана.
    /// </summary>
    public int Mana = 50;

    /// <summary>
    /// Максимальная мана.
    /// </summary>
    public int MaxMana = 50;

    /// <summary>
    /// Количество золота.
    /// </summary>
    public int Gold = 0;

    /// <summary>
    /// Имя сцены, в которой игрок находится или должен быть загружен.
    /// </summary>
    public string SceneName = "Game";

    /// <summary>
    /// Позиция игрока по оси X.
    /// </summary>
    public float PositionX = 0f;

    /// <summary>
    /// Позиция игрока по оси Y.
    /// </summary>
    public float PositionY = 0f;
}