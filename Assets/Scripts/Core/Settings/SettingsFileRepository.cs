using System.IO;
using UnityEngine;

/// <summary>
/// Отвечает только за файловую работу с настройками.
/// Не знает ничего о UI и не применяет настройки к системе.
/// </summary>
public sealed class SettingsFileRepository
{
    private readonly string _settingsFilePath;

    /// <summary>
    /// Создаёт репозиторий файла настроек.
    /// </summary>
    /// <param name="gameFolderName">Имя папки игры внутри persistentDataPath.</param>
    public SettingsFileRepository(string gameFolderName)
    {
        _settingsFilePath = Path.Combine(
            Application.persistentDataPath,
            gameFolderName,
            "settings.json");
    }

    /// <summary>
    /// Проверяет, существует ли файл настроек.
    /// </summary>
    public bool Exists()
    {
        return File.Exists(_settingsFilePath);
    }

    /// <summary>
    /// Сохраняет JSON в файл настроек.
    /// </summary>
    public void Save(string json)
    {
        EnsureDirectory();
        File.WriteAllText(_settingsFilePath, json);
    }

    /// <summary>
    /// Загружает JSON из файла настроек.
    /// </summary>
    public string Load()
    {
        return File.ReadAllText(_settingsFilePath);
    }

    /// <summary>
    /// Создаёт директорию, если её ещё нет.
    /// </summary>
    private void EnsureDirectory()
    {
        string directory = Path.GetDirectoryName(_settingsFilePath);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}
