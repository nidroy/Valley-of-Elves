using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Сервис для загрузки локализации и получения переводов по ключу.
/// Хранит загруженные строки в памяти.
/// </summary>
public static class LocalizationService
{
    /// <summary>
    /// Словарь перевода: ключ локализации -> текст перевода.
    /// </summary>
    private static readonly Dictionary<string, string> _translations = new Dictionary<string, string>();



    /// <summary>
    /// Признак того, что локализация успешно загружена.
    /// </summary>
    public static bool IsLoaded { get; private set; }



    /// <summary>
    /// Метод загружает локализацию из Resources/Localization/{languageCode}.
    /// Ожидается JSON-файл, содержащий список записей локализации.
    /// </summary>
    /// <param name="languageCode">Код языка, например "en" или "ru".</param>
    /// <returns>true, если локализация загружена успешно, иначе false.</returns>
    public static bool LoadLocalization(string languageCode)
    {
        if (string.IsNullOrWhiteSpace(languageCode))
        {
            LogError("Language code cannot be empty.");
            return false;
        }

        // Загружаем JSON-файл локализации из папки Resources.
        TextAsset file = Resources.Load<TextAsset>($"Localization/{languageCode}");

        if (file == null)
        {
            LogError($"Localization file not found: {languageCode}");
            return false;
        }

        // Десериализуем JSON в объект с данными локализации.
        LocalizationFileData data = JsonUtility.FromJson<LocalizationFileData>(file.text);

        if (data == null || data.LocalizationEntries == null)
        {
            LogError("Localization data is invalid.");
            return false;
        }

        // Временный словарь для проверки корректности данных.
        Dictionary<string, string> loaded = new Dictionary<string, string>();

        foreach (LocalizationEntry entry in data.LocalizationEntries)
        {
            if (entry == null || string.IsNullOrWhiteSpace(entry.Key))
            {
                LogWarning("Invalid localization entry skipped.");
                continue;
            }

            loaded[entry.Key] = entry.Text ?? string.Empty;
        }

        if (loaded.Count == 0)
        {
            LogWarning("Localization contains no valid entries.");
            return false;
        }

        // Очищаем старые переводы и записываем новые.
        _translations.Clear();

        foreach (KeyValuePair<string, string> pair in loaded)
        {
            _translations.Add(pair.Key, pair.Value);
        }

        IsLoaded = true;

        LogInfo($"Localization loaded: {languageCode}");
        return true;
    }

    /// <summary>
    /// Метод возвращает перевод по ключу.
    /// Если ключ не найден, возвращает сам ключ как fallback.
    /// </summary>
    /// <param name="key">Ключ локализации.</param>
    /// <returns>Переведённая строка или сам ключ, если перевод не найден.</returns>
    public static string GetTranslation(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            LogError("Translation key cannot be empty.");
            return string.Empty;
        }

        if (_translations.TryGetValue(key, out string translation))
        {
            return translation;
        }

        LogWarning($"Translation key not found: {key}");
        return key;
    }



    /// <summary>
    /// Метод записывает информационное сообщение в лог.
    /// </summary>
    private static void LogInfo(string message)
    {
        Logger.Log(LogLevel.Info, nameof(LocalizationService), message);
    }

    /// <summary>
    /// Метод записывает предупреждение в лог.
    /// </summary>
    private static void LogWarning(string message)
    {
        Logger.Log(LogLevel.Warning, nameof(LocalizationService), message);
    }

    /// <summary>
    /// Метод записывает ошибку в лог.
    /// </summary>
    private static void LogError(string message)
    {
        Logger.Log(LogLevel.Error, nameof(LocalizationService), message);
    }
}