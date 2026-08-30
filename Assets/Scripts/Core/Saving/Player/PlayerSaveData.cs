using System;

/// <summary>
/// Модель данных сохранения игрока.
/// </summary>
[Serializable]
public class PlayerSaveData
{
    /// <summary>
    /// Версия структуры сохранения игрока.
    /// </summary>
    public int Version = Globals.PlayerSaveDefaultVersion;



    /// <summary>
    /// Имя игрока.
    /// </summary>
    public string Name = Globals.PlayerSaveDefaultName;

    /// <summary>
    /// Класс или роль игрока.
    /// </summary>
    public string Class = Globals.PlayerSaveDefaultClass;

    /// <summary>
    /// Текст класса игрока (отображаемый класс).
    /// </summary>
    public string ClassText = Globals.PlayerSaveDefaultClassText;



    /// <summary>
    /// Текущий уровень игрока.
    /// </summary>
    public int Level = Globals.PlayerSaveDefaultLevel;

    /// <summary>
    /// Текущее количество опыта игрока.
    /// </summary>
    public int Experience = Globals.PlayerSaveDefaultExperience;



    /// <summary>
    /// Текущее здоровье игрока.
    /// </summary>
    public int Health = Globals.PlayerSaveDefaultHealth;

    /// <summary>
    /// Максимальное значение здоровья игрока.
    /// </summary>
    public int MaxHealth = Globals.PlayerSaveDefaultMaxHealth;



    /// <summary>
    /// Текущее количество маны игрока.
    /// </summary>
    public int Mana = Globals.PlayerSaveDefaultMana;

    /// <summary>
    /// Максимальное количество маны игрока.
    /// </summary>
    public int MaxMana = Globals.PlayerSaveDefaultMaxMana;



    /// <summary>
    /// Количество золота у игрока.
    /// </summary>
    public int Gold = Globals.PlayerSaveDefaultGold;



    /// <summary>
    /// Позиция игрока по оси X.
    /// </summary>
    public float PositionX = Globals.PlayerSaveDefaultPositionX;

    /// <summary>
    /// Позиция игрока по оси Y.
    /// </summary>
    public float PositionY = Globals.PlayerSaveDefaultPositionY;
}