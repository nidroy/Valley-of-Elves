using System.IO;
using UnityEngine;

/// <summary>
/// Отвечает за работу с файлом сохранения.
/// Только чтение, запись, удаление и проверка существования файла.
/// </summary>
public class SaveGameFileRepository
{
    #region Поля

    /// <summary>
    /// Имя файла сохранения.
    /// </summary>
    private const string FileName = "savegame.json";

    #endregion


    #region Публичные методы

    /// <summary>
    /// Сохраняет строку JSON в файл.
    /// </summary>
    /// <param name="json">JSON-данные сохранения.</param>
    public void Save(string json)
    {
        string filePath = GetFilePath();
        File.WriteAllText(filePath, json);
    }

    /// <summary>
    /// Загружает JSON из файла.
    /// </summary>
    /// <returns>Содержимое файла или null, если файла нет.</returns>
    public string Load()
    {
        string filePath = GetFilePath();

        if (!File.Exists(filePath))
        {
            return null;
        }

        return File.ReadAllText(filePath);
    }

    /// <summary>
    /// Удаляет файл сохранения, если он существует.
    /// </summary>
    public void Delete()
    {
        string filePath = GetFilePath();

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    /// <summary>
    /// Проверяет, существует ли файл сохранения.
    /// </summary>
    /// <returns>True, если файл существует.</returns>
    public bool HasSaveFile()
    {
        return File.Exists(GetFilePath());
    }

    #endregion


    #region Вспомогательные методы

    /// <summary>
    /// Возвращает полный путь к файлу сохранения.
    /// </summary>
    /// <returns>Полный путь к файлу.</returns>
    private string GetFilePath()
    {
        return Path.Combine(Application.persistentDataPath, FileName);
    }

    #endregion
}