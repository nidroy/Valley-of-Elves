using UnityEngine;

/// <summary>
/// Валидатор данных настроек.
/// Нормализатор приводит значения к безопасным диапазонам.
/// </summary>
public static class SettingsDataValidator
{
    /// <summary>
    /// Метод приводит данные настроек к безопасному и корректному состоянию.
    /// </summary>
    /// <param name="data">Объект данных настроек, который нужно проверить и исправить.</param>
    public static void Normalize(SettingsData data)
    {
        // Если объект не передан, ничего не делаем.
        if (data == null)
        {
            return;
        }

        // Версия не может быть меньше 1.
        data.Version = Mathf.Max(1, data.Version);

        // Громкость ограничивается диапазоном от 0 до 100.
        data.MusicVolume = Mathf.Clamp(data.MusicVolume, 0f, 100f);
        data.SoundVolume = Mathf.Clamp(data.SoundVolume, 0f, 100f);

        // Если разрешение не задано, используем значение по умолчанию.
        if (string.IsNullOrWhiteSpace(data.ScreenResolution))
        {
            data.ScreenResolution = Globals.SettingsDefaultScreenResolution;
        }

        // Если локализация не задана, используем значение по умолчанию.
        if (string.IsNullOrWhiteSpace(data.Localization))
        {
            data.Localization = Globals.SettingsDefaultLocalization;
        }

        // Если код языка не задан, используем значение по умолчанию.
        if (string.IsNullOrWhiteSpace(data.LanguageCode))
        {
            data.LanguageCode = Globals.SettingsDefaultLanguageCode;
        }
    }
}