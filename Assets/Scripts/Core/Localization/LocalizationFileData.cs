using System;
using System.Collections.Generic;

/// <summary>
/// Контейнер для данных локализации, загружаемых из файла.
/// Содержит список всех записей локализации.
/// </summary>
[Serializable]
public class LocalizationFileData
{
    /// <summary>
    /// Список записей локализации.
    /// Каждая запись содержит ключ и соответствующий текст.
    /// </summary>
    public List<LocalizationEntry> LocalizationEntries;
}