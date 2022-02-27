namespace TeenControlSystemWeb.Data.Repositories;

public interface IDataProvider
{
    public IRepository<Point> PointsRepository { get; }
    public IRepository<Sensor> SensorsRepository { get; }
    public IRepository<Session> SessionsRepository { get; }
    public IRepository<User> UsersRepository { get; }
    public IRepository<UserAuthorizationToken> UserAuthorizationTokensRepository { get; }

    public Task SaveChangesAsync();
}