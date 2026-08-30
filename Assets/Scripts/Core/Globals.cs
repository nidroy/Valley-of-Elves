using System.IO;
using UnityEngine;

/// <summary>
/// Глобальные флаги и значения, используемые разными системами игры.
/// </summary>
public static class Globals
{
    #region Constants

    /// <summary>
    /// Имя папки игры.
    /// </summary>
    private const string _gameFolderName = "Valley of Elves";



    /// <summary>
    /// Имя папки, в которой хранятся логи.
    /// </summary>
    private const string _logsFolderName = "Logs";

    /// <summary>
    /// Имя файла логов.
    /// </summary>
    private const string _logFileName = "game.log";



    /// <summary>
    /// Имя папки, в которой хранятся настройки игры.
    /// </summary>
    private const string _gameSettingsFolderName = "Game settings";

    /// <summary>
    /// Имя файла настроек игры.
    /// </summary>
    private const string _gameSettingsFileName = "gamesettings.json";



    /// <summary>
    /// Имя папки, в которой хранятся сохранения игры.
    /// </summary>
    private const string _gameSavesFolderName = "Game saves";

    /// <summary>
    /// Имя файла сохранения игры.
    /// </summary>
    private const string _gameSaveFileName = "gamesave.json";

    /// <summary>
    /// Имя файла сохранения игрока.
    /// </summary>
    private const string _playerSaveFileName = "playersave.json";

    #endregion



    #region Log Settings

    /// <summary>
    /// Максимальный размер файла логов в мегабайтах.
    /// </summary>
    public const int MaxLogFileSizeMB = 5;

    /// <summary>
    /// Полный путь к файлу логов.
    /// </summary>
    public static readonly string LogFilePath =
        Path.Combine(
            Application.persistentDataPath,
            _gameFolderName,
            _logsFolderName,
            _logFileName);

    #endregion



    #region Game Settings

    /// <summary>
    /// Полный путь к файлу настроек игры.
    /// </summary>
    public static readonly string GameSettingsFilePath =
        Path.Combine(
            Application.persistentDataPath,
            _gameFolderName,
            _gameSettingsFolderName,
            _gameSettingsFileName);

    #endregion



    #region Game Save Settings

    /// <summary>
    /// Полный путь к файлу сохранения игры.
    /// </summary>
    public static readonly string GameSaveFilePath =
        Path.Combine(
            Application.persistentDataPath,
            _gameFolderName,
            _gameSavesFolderName,
            _gameSaveFileName);

    /// <summary>
    /// Полный путь к файлу сохранения игрока.
    /// </summary>
    public static readonly string PlayerSaveFilePath =
        Path.Combine(
            Application.persistentDataPath,
            _gameFolderName,
            _gameSavesFolderName,
            _playerSaveFileName);

    #endregion



    #region Default Settings Values

    /// <summary>
    /// Значение версии настроек по умолчанию.
    /// </summary>
    public const int SettingsDefaultVersion = 1;



    /// <summary>
    /// Разрешение экрана по умолчанию.
    /// </summary>
    public const string SettingsDefaultScreenResolution = "1920x1080";

    /// <summary>
    /// Значение полноэкранного режима по умолчанию.
    /// </summary>
    public const bool SettingsDefaultIsFullScreen = true;



    /// <summary>
    /// Громкость музыки по умолчанию.
    /// </summary>
    public const float SettingsDefaultMusicVolume = 100f;

    /// <summary>
    /// Громкость звуков по умолчанию.
    /// </summary>
    public const float SettingsDefaultSoundVolume = 100f;



    /// <summary>
    /// Название локализации по умолчанию.
    /// </summary>
    public const string SettingsDefaultLocalization = "English";

    /// <summary>
    /// Код языка по умолчанию.
    /// </summary>
    public const string SettingsDefaultLanguageCode = "EN";



    /// <summary>
    /// Значение включённой записи логов в файл по умолчанию.
    /// </summary>
    public const bool SettingsDefaultIsFileLogging = true;

    #endregion



    #region Default Game Save Values

    /// <summary>
    /// Значение версии сохранения игры по умолчанию.
    /// </summary>
    public const int GameSaveDefaultVersion = 1;



    /// <summary>
    /// Название сцены по умолчанию.
    /// </summary>
    public const string GameSaveDefaultSceneName = "Village Scene";

    #endregion



    #region Default Player Save Values

    /// <summary>
    /// Значение версии сохранения игрока по умолчанию.
    /// </summary>
    public const int PlayerSaveDefaultVersion = 1;



    /// <summary>
    /// Имя игрока по умолчанию.
    /// </summary>
    public const string PlayerSaveDefaultName = "Player";

    /// <summary>
    /// Класс игрока по умолчанию.
    /// </summary>
    public const string PlayerSaveDefaultClass = "Adventurer";

    /// <summary>
    /// Текст класса игрока (отображаемый класс).
    /// </summary>
    public const string PlayerSaveDefaultClassText = "Adventurer";



    /// <summary>
    /// Уровень игрока по умолчанию.
    /// </summary>
    public const int PlayerSaveDefaultLevel = 1;

    /// <summary>
    /// Опыт игрока по умолчанию.
    /// </summary>
    public const int PlayerSaveDefaultExperience = 0;



    /// <summary>
    /// Здоровье игрока по умолчанию.
    /// </summary>
    public const int PlayerSaveDefaultHealth = 100;

    /// <summary>
    /// Максимальное здоровье игрока по умолчанию.
    /// </summary>
    public const int PlayerSaveDefaultMaxHealth = 100;



    /// <summary>
    /// Мана игрока по умолчанию.
    /// </summary>
    public const int PlayerSaveDefaultMana = 50;

    /// <summary>
    /// Максимальная мана игрока по умолчанию.
    /// </summary>
    public const int PlayerSaveDefaultMaxMana = 50;



    /// <summary>
    /// Золото игрока по умолчанию.
    /// </summary>
    public const int PlayerSaveDefaultGold = 0;



    /// <summary>
    /// Позиция игрока по оси X по умолчанию.
    /// </summary>
    public const float PlayerSaveDefaultPositionX = 0f;

    /// <summary>
    /// Позиция игрока по оси Y по умолчанию.
    /// </summary>
    public const float PlayerSaveDefaultPositionY = 0f;

    #endregion
}