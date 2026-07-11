using System;

/// <summary>
/// Модель данных пользовательских настроек.
/// Содержит только данные, без логики сохранения или применения.
/// </summary>
[Serializable]
public class SettingsData
{
    /// <summary>
    /// Версия структуры настроек.
    /// Используется для возможной миграции формата в будущем.
    /// </summary>
    public int Version = 1;

    /// <summary>
    /// Разрешение экрана в формате "ШиринаxВысота".
    /// Пример: "1920x1080".
    /// </summary>
    public string ScreenResolution = "1920x1080";

    /// <summary>
    /// Включён ли полноэкранный режим.
    /// </summary>
    public bool IsFullScreen = true;

    /// <summary>
    /// Громкость музыки в диапазоне от 0 до 100.
    /// </summary>
    public float MusicVolume = 100f;

    /// <summary>
    /// Громкость звуков в диапазоне от 0 до 100.
    /// </summary>
    public float SoundVolume = 100f;

    /// <summary>
    /// Название текущей локализации.
    /// Пример: "English", "Русский".
    /// </summary>
    public string Localization = "English";

    /// <summary>
    /// Код языка.
    /// Пример: "EN", "RU".
    /// </summary>
    public string LanguageCode = "EN";

    /// <summary>
    /// Включена ли запись логов в файл.
    /// </summary>
    public bool IsFileLogging = true;

    /// <summary>
    /// Индекс уровня качества графики.
    /// </summary>
    public int QualityLevel = 3;

    /// <summary>
    /// Включён ли VSync.
    /// </summary>
    public bool IsVSync = true;

    /// <summary>
    /// Ограничение частоты кадров.
    /// </summary>
    public int FrameRate = 60;
}