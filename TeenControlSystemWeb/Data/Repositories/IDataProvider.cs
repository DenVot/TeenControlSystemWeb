namespace TeenControlSystemWeb.Data.Repositories;

/// <summary>
/// Набор репозиториев
/// </summary>
public interface IDataProvider
{
    public IRepository<Point> PointsRepository { get; }
    public IRepository<Sensor> SensorsRepository { get; }
    public IRepository<Session> SessionsRepository { get; }
    public IRepository<User> UsersRepository { get; }
    public IRepository<DefaultAvatar> DefaultAvatarsRepository { get; }

    public Task SaveChangesAsync();
}