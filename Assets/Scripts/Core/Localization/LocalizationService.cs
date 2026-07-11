using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Сервис загрузки локализации и получения перевода.
/// </summary>
public static class LocalizationService
{
    private static readonly Dictionary<string, string> Translations =
        new Dictionary<string, string>();

    /// <summary>
    /// Признак того, что локализация загружена.
    /// </summary>
    public static bool IsLoaded { get; private set; }

    /// <summary>
    /// Загружает локализацию из Resources/Localization/{languageCode}.
    /// </summary>
    public static bool LoadLocalization(string languageCode)
    {
        if (string.IsNullOrWhiteSpace(languageCode))
        {
            LogError("Language code cannot be empty.");
            return false;
        }

        TextAsset file = Resources.Load<TextAsset>($"Localization/{languageCode}");

        if (file == null)
        {
            LogError($"Localization file not found: {languageCode}");
            return false;
        }

        LocalizationFileData data = JsonUtility.FromJson<LocalizationFileData>(file.text);

        if (data == null || data.Words == null)
        {
            LogError("Localization data is invalid.");
            return false;
        }

        Dictionary<string, string> loaded = new Dictionary<string, string>();

        foreach (LocalizationEntry entry in data.Words)
        {
            if (entry == null || string.IsNullOrWhiteSpace(entry.Key))
            {
                LogWarning("Invalid localization entry skipped.");
                continue;
            }

            loaded[entry.Key] = entry.Value ?? string.Empty;
        }

        if (loaded.Count == 0)
        {
            LogWarning("Localization contains no valid entries.");
            return false;
        }

        Translations.Clear();

        foreach (KeyValuePair<string, string> pair in loaded)
        {
            Translations.Add(pair.Key, pair.Value);
        }

        IsLoaded = true;
        LogInfo($"Localization loaded: {languageCode}");
        return true;
    }

    /// <summary>
    /// Возвращает перевод по ключу.
    /// </summary>
    public static string GetTranslation(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            LogError("Translation key cannot be empty.");
            return string.Empty;
        }

        if (Translations.TryGetValue(key, out string translation))
        {
            return translation;
        }

        LogWarning($"Translation key not found: {key}");
        return key;
    }

    private static void LogInfo(string message)
    {
        Logger.Log(LogLevel.Info, nameof(LocalizationService), message);
    }

    private static void LogWarning(string message)
    {
        Logger.Log(LogLevel.Warning, nameof(LocalizationService), message);
    }

    private static void LogError(string message)
    {
        Logger.Log(LogLevel.Error, nameof(LocalizationService), message);
    }
}