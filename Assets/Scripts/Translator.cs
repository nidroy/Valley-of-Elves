using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс управляет загрузкой и получением локализованных текстов игры.
/// </summary>
public static class Translator
{
    #region Приватные классы

    /// <summary>
    /// Класс хранит данные одного переведенного слова.
    /// </summary>
    [Serializable]
    private class WordData
    {
        // Ключ слова.
        public string Key;

        // Переведенное значение.
        public string Value;
    }

    /// <summary>
    /// Класс хранит список всех переведенных слов.
    /// </summary>
    [Serializable]
    private class TranslationData
    {
        // Коллекция переводов.
        public List<WordData> Words;
    }

    #endregion


    #region Приватные поля

    // Словарь текущей локализации.
    private static readonly Dictionary<string, string> _translation = new Dictionary<string, string>();

    #endregion


    #region Публичные методы

    /// <summary>
    /// Метод загружает локализацию из файла.
    /// </summary>
    /// <param name="languageCode">Код загружаемого языка.</param>
    public static void LoadLocalization(string languageCode)
    {
        try
        {
            // Проверяем корректность кода языка.
            if (!ValidateLanguageCode(languageCode))
            {
                LogError("Language code cannot be empty.");
                return;
            }

            // Получаем файл локализации.
            TextAsset localizationFile = LoadLocalizationFile(languageCode);

            // Проверяем наличие файла.
            if (localizationFile == null)
            {
                LogError($"Localization file for '{languageCode}' was not found.");
                return;
            }

            // Очищаем предыдущие переводы.
            ClearTranslation();

            // Загружаем переводы из JSON.
            TranslationData translationData = DeserializeLocalization(localizationFile.text);

            // Проверяем результат загрузки.
            if (translationData == null || translationData.Words == null)
            {
                LogError("Localization data is empty or invalid.");
                return;
            }

            // Добавляем переводы в словарь.
            AddTranslations(translationData.Words);

            LogInfo($"Localization loaded: {languageCode}.");
        }
        catch (Exception exception)
        {
            LogError($"Failed to load localization: {exception.Message}");
        }
    }

    /// <summary>
    /// Метод получает перевод по ключу.
    /// </summary>
    /// <param name="keyWord">Ключ перевода.</param>
    /// <returns>Переведенный текст.</returns>
    public static string Translation(string keyWord)
    {
        try
        {
            // Проверяем корректность ключа.
            if (!ValidateLanguageCode(keyWord))
            {
                LogError("Translation key cannot be empty.");
                return string.Empty;
            }

            // Проверяем наличие перевода.
            if (_translation.TryGetValue(keyWord, out string translatedWord))
            {
                return translatedWord;
            }

            LogWarning($"Missing translation for key '{keyWord}'.");

            return string.Empty;
        }
        catch (Exception exception)
        {
            LogError($"Failed to get translation: {exception.Message}");

            return string.Empty;
        }
    }

    #endregion


    #region Приватные методы загрузки

    /// <summary>
    /// Метод загружает файл локализации из ресурсов игры.
    /// </summary>
    /// <param name="languageCode">Код языка локализации.</param>
    /// <returns>Файл локализации.</returns>
    private static TextAsset LoadLocalizationFile(string languageCode)
    {
        // Загружаем файл из папки Resources/Localization.
        return Resources.Load<TextAsset>($"Localization/{languageCode}");
    }


    /// <summary>
    /// Метод преобразует JSON локализации в объект.
    /// </summary>
    /// <param name="json">JSON строка с переводами.</param>
    /// <returns>Объект данных локализации.</returns>
    private static TranslationData DeserializeLocalization(string json)
    {
        // Проверяем наличие JSON.
        if (string.IsNullOrWhiteSpace(json))
        {
            LogError("Localization JSON is empty.");
            return null;
        }

        // Преобразуем JSON в объект.
        return JsonUtility.FromJson<TranslationData>(json);
    }

    /// <summary>
    /// Метод добавляет переводы в словарь.
    /// </summary>
    /// <param name="words">Список переводимых слов.</param>
    private static void AddTranslations(List<WordData> words)
    {
        // Перебираем все переводы.
        foreach (WordData word in words)
        {
            // Проверяем корректность данных.
            if (word == null || !ValidateLanguageCode(word.Key))
            {
                LogWarning("Invalid translation entry skipped.");
                continue;
            }


            // Проверяем наличие значения.
            if (word.Value == null)
            {
                LogWarning($"Translation value for '{word.Key}' is empty.");
                continue;
            }

            // Добавляем перевод в словарь.
            _translation[word.Key] = word.Value;
        }
    }

    /// <summary>
    /// Метод очищает текущие переводы.
    /// </summary>
    private static void ClearTranslation()
    {
        // Удаляем старые значения.
        _translation.Clear();
    }

    #endregion


    #region Методы проверки

    /// <summary>
    /// Метод проверяет строковое значение.
    /// </summary>
    /// <param name="value">Проверяемая строка.</param>
    /// <returns>True, если строка содержит значение.</returns>
    private static bool ValidateLanguageCode(string value)
    {
        // Проверяем наличие текста.
        return !string.IsNullOrWhiteSpace(value);
    }

    #endregion


    #region Логирование

    /// <summary>
    /// Метод записывает информационное сообщение.
    /// </summary>
    /// <param name="message">Сообщение для записи.</param>
    private static void LogInfo(string message)
    {
        Logger.Log(Logger.LogLevel.Info, nameof(Translator), message);
    }

    /// <summary>
    /// Метод записывает предупреждение.
    /// </summary>
    /// <param name="message">Сообщение предупреждения.</param>
    private static void LogWarning(string message)
    {
        Logger.Log(Logger.LogLevel.Warning, nameof(Translator), message);
    }

    /// <summary>
    /// Метод записывает ошибку.
    /// </summary>
    /// <param name="message">Сообщение ошибки.</param>
    private static void LogError(string message)
    {
        Logger.Log(Logger.LogLevel.Error, nameof(Translator), message);
    }

    #endregion
}