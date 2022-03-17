namespace TeenControlSystemWeb.Data.Repositories;

/// <summary>
/// Абстракция для манипуляции с данными
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Находит сущность по ключу
    /// </summary>
    /// <param name="id">Ключ</param>
    Task<T?> FindAsync(object id);
    
    /// <summary>
    /// Добавляет сущность
    /// </summary>
    /// <param name="obj">Сущность</param>
    Task AddAsync(T obj);
    
    /// <summary>
    /// Добавляет сущности
    /// </summary>
    /// <param name="objs">Сущности</param>
    Task AddRangeAsync(IEnumerable<T> objs);
    
    /// <summary>
    /// Удаляет сущность
    /// </summary>
    /// <param name="target">Сущность</param>
    void Remove(T target);
    
    /// <summary>
    /// Удаляет сущности
    /// </summary>
    /// <param name="targets">Сущности</param>
    void RemoveRange(IEnumerable<T> targets);

    /// <summary>
    /// Получает все сущности
    /// </summary>
    IEnumerable<T> GetAll();
}