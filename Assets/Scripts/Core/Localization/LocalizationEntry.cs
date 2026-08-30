using System;

/// <summary>
/// Запись локализации.
/// Содержит ключ и соответствующий ему текст.
/// </summary>
[Serializable]
public class LocalizationEntry
{
    /// <summary>
    /// Уникальный ключ строки локализации.
    /// </summary>
    public string Key;

    /// <summary>
    /// Текст, который соответствует ключу локализации.
    /// </summary>
    public string Text;
}