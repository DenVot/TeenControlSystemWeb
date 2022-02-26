namespace TeenControlSystemWeb.Data.Repositories;

public interface IRepository<T> where T : class
{
    T? Find(object id);
    void Add(T obj);
    void AddRange(IEnumerable<T> objs);
    void Remove(T target);
    void RemoveRange(IEnumerable<T> targets);
}