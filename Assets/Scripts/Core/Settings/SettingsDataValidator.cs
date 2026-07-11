using UnityEngine;

/// <summary>
/// Выполняет нормализацию и проверку данных настроек.
/// </summary>
public static class SettingsDataValidator
{
    /// <summary>
    /// Приводит данные настроек к допустимым значениям.
    /// </summary>
    public static void Normalize(SettingsData data)
    {
        if (data == null)
        {
            return;
        }

        data.Version = Mathf.Max(1, data.Version);
        data.MusicVolume = Mathf.Clamp(data.MusicVolume, 0f, 100f);
        data.SoundVolume = Mathf.Clamp(data.SoundVolume, 0f, 100f);
        data.FrameRate = Mathf.Max(30, data.FrameRate);

        if (string.IsNullOrWhiteSpace(data.ScreenResolution))
        {
            data.ScreenResolution = "1920x1080";
        }

        if (string.IsNullOrWhiteSpace(data.Localization))
        {
            data.Localization = "English";
        }

        if (string.IsNullOrWhiteSpace(data.LanguageCode))
        {
            data.LanguageCode = "EN";
        }
    }
}