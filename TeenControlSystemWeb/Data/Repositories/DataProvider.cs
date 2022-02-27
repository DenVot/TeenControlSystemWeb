namespace TeenControlSystemWeb.Data.Repositories;

public class DataProvider : IDataProvider
{
    private readonly TcsDatabaseContext _context;

    public DataProvider(TcsDatabaseContext context)
    {
        _context = context;
        PointsRepository = new Repository<Point>(context);
        SensorsRepository = new Repository<Sensor>(context);
        SessionsRepository = new Repository<Session>(context);
        UsersRepository = new Repository<User>(context);
        UserAuthorizationTokensRepository = new Repository<UserAuthorizationToken>(context);
    }
    
    public IRepository<Point> PointsRepository { get; }
    public IRepository<Sensor> SensorsRepository { get; }
    public IRepository<Session> SessionsRepository { get; }
    public IRepository<User> UsersRepository { get; }
    public IRepository<UserAuthorizationToken> UserAuthorizationTokensRepository { get; }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}