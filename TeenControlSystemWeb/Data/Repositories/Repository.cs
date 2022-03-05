namespace TeenControlSystemWeb.Data.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly TcsDatabaseContext _context;

    public Repository(TcsDatabaseContext context)
    {
        _context = context;
    }

    public async Task<T?> FindAsync(object id) => await _context.FindAsync<T>(id);

    public async Task AddAsync(T obj) => await _context.AddAsync(obj);

    public async Task AddRangeAsync(IEnumerable<T> objs) => await _context.AddRangeAsync(objs);

    public void Remove(T target) => _context.Remove(target);

    public void RemoveRange(IEnumerable<T> targets) => _context.RemoveRange(targets);
    
    public IEnumerable<T> GetAll() => _context.Set<T>();
}