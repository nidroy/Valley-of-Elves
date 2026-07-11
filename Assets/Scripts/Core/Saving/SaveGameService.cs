using UnityEngine;

/// <summary>
/// Управляет сохранением и загрузкой игры.
/// Содержит бизнес-логику поверх файлового репозитория.
/// </summary>
public static class SaveGameService
{
    #region Поля

    /// <summary>
    /// Репозиторий для работы с файлом сохранения.
    /// </summary>
    private static readonly SaveGameFileRepository repository = new SaveGameFileRepository();

    /// <summary>
    /// Текущие данные сохранения.
    /// </summary>
    public static SaveGameData Current { get; private set; }

    /// <summary>
    /// Имя сцены, в которую нужно загрузиться.
    /// </summary>
    public static string SceneName { get; set; }

    #endregion


    #region Свойства

    /// <summary>
    /// Возвращает true, если сохранение существует на диске.
    /// </summary>
    public static bool HasSave => repository.HasSaveFile();

    #endregion


    #region Публичные методы

    /// <summary>
    /// Создаёт новое сохранение в памяти.
    /// </summary>
    public static void CreateNew()
    {
        Current = new SaveGameData();
    }

    /// <summary>
    /// Сохраняет текущие данные сохранения в файл.
    /// </summary>
    public static void Save()
    {
        if (Current == null)
        {
            Current = new SaveGameData();
        }

        string json = JsonUtility.ToJson(Current, true);
        repository.Save(json);
    }

    /// <summary>
    /// Загружает сохранение из файла.
    /// </summary>
    /// <returns>True, если загрузка прошла успешно.</returns>
    public static bool Load()
    {
        string json = repository.Load();

        if (string.IsNullOrWhiteSpace(json))
        {
            Current = null;
            return false;
        }

        Current = JsonUtility.FromJson<SaveGameData>(json);
        return Current != null;
    }

    /// <summary>
    /// Удаляет сохранение и очищает текущие данные.
    /// </summary>
    public static void Delete()
    {
        repository.Delete();
        Current = null;
    }

    #endregion
}