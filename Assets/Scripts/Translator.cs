using System;
using System.Collections.Generic;
using UnityEngine;

public static class Translator
{
    // Переведенные слова согласно локализации
    private static Dictionary<string, string> _translation = new Dictionary<string, string>();

    /// <summary>
    /// Класс для хранения данных переведенного слова
    /// </summary>
    [Serializable]
    private class WordData
    {
        public string Key;      // Ключевое слово
        public string Value;    // Переведенное слово
    }

    /// <summary>
    /// Класс для хранения данных всех переведенных слов
    /// </summary>
    [Serializable]
    private class TranslationData
    {
        public List<WordData> Words;     // Перевод слов
    }

    /// <summary>
    /// Метод для загрузки локализации из файла в список переведенных слов
    /// </summary>
    /// <param name="languageCode">Код языка</param>
    public static void LoadLocalization(string languageCode)
    {
        // Загрузка файла локализации из Resources
        TextAsset localization = Resources.Load<TextAsset>($"Localization/{languageCode}");

        // Заполнение словаря переведенных слов
        var translation = JsonUtility.FromJson<TranslationData>(localization.text);
        foreach (var word in translation.Words)
            _translation[word.Key] = word.Value;
    }

    /// <summary>
    /// Метод для перевода слова согласно локализации
    /// </summary>
    /// <param name="keyWord">Ключевое слово</param>
    /// <returns>Переведенное слово</returns>
    public static string Translation(string keyWord)
    {
        if (_translation != null && _translation.TryGetValue(keyWord, out string word))
            return word;

        LogError($"Missing translation for key '{keyWord}' in current localization.");
        return string.Empty;
    }


    /// <summary>
    /// Метод для логирования ошибок
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private static void LogError(string message)
    {
        Logger.Log(Logger.LogLevel.Error, nameof(Settings), message);
    }

    /// <summary>
    /// Метод для логирования предупреждений
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private static void LogWarning(string message)
    {
        Logger.Log(Logger.LogLevel.Warning, nameof(Settings), message);
    }

    /// <summary>
    /// Метод для логирования информационных сообщений
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private static void LogInfo(string message)
    {
        Logger.Log(Logger.LogLevel.Info, nameof(Settings), message);
    }
}
