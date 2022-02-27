namespace TeenControlSystemWeb.Data.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> FindAsync(object id);
    Task AddAsync(T obj);
    Task AddRangeAsync(IEnumerable<T> objs);
    Task RemoveAsync(T target);
    Task RemoveRangeAsync(IEnumerable<T> targets);
}