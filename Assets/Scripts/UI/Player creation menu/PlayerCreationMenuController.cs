using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Контроллер меню создания игрока.
/// Отвечает за временные данные, валидацию, применение и сброс.
/// </summary>
public class PlayerCreationMenuController
{
    #region Поля

    /// <summary>
    /// Временные данные игрока.
    /// </summary>
    private TempPlayerData _tempPlayerData = new TempPlayerData();

    /// <summary>
    /// Признак того, что игрок считается созданным.
    private bool _isPlayerCreated;

    /// <summary>
    /// Регулярное выражение для проверки допустимых символов в имени игрока.
    /// Разрешены буквы, цифры, пробел и подчёркивание.
    /// </summary>
    private const string PlayerNamePattern = @"^[a-zA-Zа-яА-Я0-9 _-]+$";

    #endregion



    #region Свойства

    /// <summary>
    /// Временные данные игрока.
    /// </summary>
    public TempPlayerData TempPlayerData
    {
        get => _tempPlayerData;
        private set => _tempPlayerData = value;
    }

    /// <summary>
    ///Признак того, что игрок считается созданным.
    /// </summary>
    public bool IsPlayerCreated => _isPlayerCreated;

    #endregion



    #region Применение данных игрока

    /// <summary>
    /// Метод применяет временные данные игрока к текущим данным игрока, хранящимся в памяти.
    /// </summary>
    public void ApplyPlayerData()
    {
        PlayerSaveService.Default();

        PlayerSaveService.Name = _tempPlayerData.PlayerName;
        PlayerSaveService.Class = _tempPlayerData.PlayerClass;
        PlayerSaveService.ClassText = _tempPlayerData.PlayerClassText;
        PlayerSaveService.Health = _tempPlayerData.PlayerMaxHealth;
        PlayerSaveService.MaxHealth = _tempPlayerData.PlayerMaxHealth;
        PlayerSaveService.Mana = _tempPlayerData.PlayerMaxMana;
        PlayerSaveService.MaxMana = _tempPlayerData.PlayerMaxMana;

        LogInfo("Temporary player data applied to in-memory player data");
    }



    /// <summary>
    /// Метод применяет имя игрока.
    /// </summary>
    /// <param name="playerName">Имя игрока.</param>
    public void ApplyPlayerName(string playerName)
    {
        string trimmedName = playerName?.Trim() ?? string.Empty;

        if (!IsValidPlayerName(trimmedName))
        {
            LogWarning($"Invalid player name: {playerName}");
            _tempPlayerData.PlayerName = string.Empty;
            UpdatePlayerCreatedState();
            return;
        }

        _tempPlayerData.PlayerName = trimmedName;
        LogInfo($"Player name applied: {trimmedName}");

        UpdatePlayerCreatedState();
    }

    /// <summary>
    /// Метод применяет класс игрока.
    /// </summary>
    /// <param name="className">Название класса.</param>
    /// <param name="classText">Отображаемый текст класса.</param>
    /// <param name="maxHealth">Максимальное значение здоровья класса игрока.</param>
    /// <param name="maxMana">Максимальное значение маны класса игрока.</param>
    public void ApplyPlayerClass(string className, string classText, int maxHealth, int maxMana)
    {
        _tempPlayerData.PlayerClass = className?.Trim() ?? string.Empty;
        _tempPlayerData.PlayerClassText = classText?.Trim() ?? string.Empty;
        _tempPlayerData.PlayerMaxHealth = maxHealth;
        _tempPlayerData.PlayerMaxMana = maxMana;

        LogInfo($"Player class applied: {className}");

        UpdatePlayerCreatedState();
    }

    #endregion



    #region Проверка готовности создания игрока

    /// <summary>
    /// Метод возвращает состояние создания игрока.    
    /// </summary>
    /// <returns>True, если игрок считается созданным; иначе false.</returns>
    private bool PlayerCreatedState()
    {
        return !string.IsNullOrWhiteSpace(_tempPlayerData.PlayerName)
               && IsValidPlayerName(_tempPlayerData.PlayerName)
               && !string.IsNullOrWhiteSpace(_tempPlayerData.PlayerClass);
    }

    /// <summary>
    /// Метод обновляет флаг состояния создания игрока.
    /// </summary>
    private void UpdatePlayerCreatedState()
    {
        _isPlayerCreated = PlayerCreatedState();
        LogInfo($"Player creation state updated: {_isPlayerCreated}");
    }

    #endregion



    #region Проверка имени игрока

    /// <summary>
    /// Метод проверяет имя игрока на допустимые символы.
    /// </summary>
    /// <param name="playerName">Имя игрока.</param>
    /// <returns>True, если имя допустимо; иначе false.</returns>
    private bool IsValidPlayerName(string playerName)
    {
        if (string.IsNullOrWhiteSpace(playerName))
            return false;

        return Regex.IsMatch(playerName, PlayerNamePattern);
    }

    #endregion



    #region Логирование

    /// <summary>
    ///  Метод записывает информационное сообщение в лог.
    /// </summary>
    private void LogInfo(string message)
    {
        Logger.Log(LogLevel.Info, nameof(PlayerCreationMenuController), message);
    }

    /// <summary>
    /// Метод записывает предупреждение в лог.
    /// </summary>
    private void LogWarning(string message)
    {
        Logger.Log(LogLevel.Warning, nameof(PlayerCreationMenuController), message);
    }

    /// <summary>
    /// Метод записывает ошибку в лог.
    /// </summary>
    private void LogError(string message)
    {
        Logger.Log(LogLevel.Error, nameof(PlayerCreationMenuController), message);
    }

    #endregion
}



#region Структуры

/// <summary>
/// Структура для временных данных игрока.
/// </summary>
[Serializable]
public struct TempPlayerData
{
    /// <summary>
    /// Имя игрока.
    /// </summary>
    public string PlayerName;

    /// <summary>
    /// Класс игрока.
    /// </summary>
    public string PlayerClass;

    /// <summary>
    /// Текст класса игрока.
    /// </summary>
    public string PlayerClassText;



    /// <summary>
    /// Максимальное значение здоровья игрока.
    /// </summary>
    public int PlayerMaxHealth;

    /// <summary>
    /// Максимальное количество маны игрока.
    /// </summary>
    public int PlayerMaxMana;



    /// <summary>
    /// Конструктор создаёт новый вариант временных данных игрока.
    /// </summary>
    /// <param name="playerName">Имя игрока.</param>
    /// <param name="playerClass">Класс игрока.</param>
    /// <param name="playerClassText">Текст класса игрока.</param>
    /// <param name="playerMaxHealth">Максимальное значение здоровья игрока.</param>
    /// <param name="playerMaxMana">Максимальное количество маны игрока.</param>
    public TempPlayerData(string playerName, string playerClass, string playerClassText, int playerMaxHealth, int playerMaxMana)
    {
        PlayerName = playerName;
        PlayerClass = playerClass;
        PlayerClassText = playerClassText;
        PlayerMaxHealth = playerMaxHealth;
        PlayerMaxMana = playerMaxMana;
    }
}


/// <summary>
/// Структура для карточки класса игрока.
/// </summary>
[Serializable]
public struct PlayerClassCard
{
    /// <summary>
    /// Название класса.
    /// </summary>
    public string ClassName;

    //// <summary>
    /// Кнопка выбора класса.
    /// </summary>
    public Button SelectButton;

    /// <summary>
    /// Текст класса.
    /// </summary>
    public TMP_Text ClassText;



    /// <summary>
    /// Максимальное значение здоровья класса игрока.
    /// </summary>
    public int MaxHealth;

    /// <summary>
    /// Максимальное количество маны класса игрока.
    /// </summary>
    public int MaxMana;



    /// <summary>
    /// Конструктор создаёт новый вариант карточки класса игрока.
    /// </summary>
    /// <param name="className">Название класса.</param>
    /// <param name="selectButton">Кнопка выбора класса.</param>
    /// <param name="classText">Текст класса.</param>
    /// <param name="maxHealth">Максимальное значение здоровья класса игрока.</param>
    /// <param name="maxMana">Максимальное количество маны класса игрока.</param>
    public PlayerClassCard(string className, Button selectButton, TMP_Text classText, int maxHealth, int maxMana)
    {
        ClassName = className;
        SelectButton = selectButton;
        ClassText = classText;
        MaxHealth = maxHealth;
        MaxMana = maxMana;
    }
}

#endregion