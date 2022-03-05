namespace TeenControlSystemWeb.Data.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> FindAsync(object id);
    Task AddAsync(T obj);
    Task AddRangeAsync(IEnumerable<T> objs);
    void Remove(T target);
    void RemoveRange(IEnumerable<T> targets);

    IEnumerable<T> GetAll();
}