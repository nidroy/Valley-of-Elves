using System;

/// <summary>
/// Модель данных настроек.
/// </summary>
[Serializable]
public class SettingsData
{
    /// <summary>
    /// Версия структуры настроек.
    /// </summary>
    public int Version = Globals.SettingsDefaultVersion;



    /// <summary>
    /// Разрешение экрана в формате "ШиринаxВысота".
    /// </summary>
    public string ScreenResolution = Globals.SettingsDefaultScreenResolution;

    /// <summary>
    /// Включён ли полноэкранный режим.
    /// </summary>
    public bool IsFullScreen = Globals.SettingsDefaultIsFullScreen;



    /// <summary>
    /// Громкость музыки в диапазоне от 0 до 100.
    /// </summary>
    public float MusicVolume = Globals.SettingsDefaultMusicVolume;

    /// <summary>
    /// Громкость звуков в диапазоне от 0 до 100.
    /// </summary>
    public float SoundVolume = Globals.SettingsDefaultSoundVolume;



    /// <summary>
    /// Название текущей локализации.
    /// </summary>
    public string Localization = Globals.SettingsDefaultLocalization;

    /// <summary>
    /// Код языка.
    /// </summary>
    public string LanguageCode = Globals.SettingsDefaultLanguageCode;



    /// <summary>
    /// Включена ли запись логов в файл.
    /// </summary>
    public bool IsFileLogging = Globals.SettingsDefaultIsFileLogging;
}