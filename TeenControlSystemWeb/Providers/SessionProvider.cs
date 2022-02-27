using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions.Sensor;
using TeenControlSystemWeb.Exceptions.Session;
using TeenControlSystemWeb.Exceptions.User;
using TeenControlSystemWeb.Extensions;
using TeenControlSystemWeb.Types;

namespace TeenControlSystemWeb.Providers;

public class SessionProvider
{
    private readonly IDataProvider _dataProvider;
    private readonly IRepository<User> _usersRepository;
    private readonly IRepository<Sensor> _sensorsRepository;
    private readonly IRepository<Session> _sessionsRepository;
    private readonly IRepository<Point> _pointsRepository;

    public SessionProvider(IDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
        _usersRepository = dataProvider.UsersRepository;
        _sensorsRepository = dataProvider.SensorsRepository;
        _sessionsRepository = dataProvider.SessionsRepository;
        _pointsRepository = dataProvider.PointsRepository;
    }
    
    /// <summary>
    /// Регистрирует сессию
    /// </summary>
    /// <param name="userId">Id владельца сессии</param>
    /// <param name="sessionName">Назавание сессии</param>
    /// <param name="startAt">Запланированное начало сессии</param>
    /// <param name="sensorsIds">Id всех сенсоров, которые учавствуют в сессии</param>
    /// <param name="fromPointType">Начальная точка сессии</param>
    /// <param name="toPointType">Конечная точка сессии</param>
    /// <exception cref="UserNotFoundException">Вызывается, если пользователь не найден</exception>
    /// <exception cref="UserAlreadyInUseException">Вызывается, если пользователь уже учавствует в другой сессии</exception>
    public void RegisterSession(long userId,
        string sessionName,
        DateTime startAt,
        IEnumerable<long> sensorsIds,
        PointType fromPointType,
        PointType toPointType)
    {
        var targetUser = _usersRepository.Find(userId);

        if (targetUser == null)
        {
            throw new UserNotFoundException(userId);
        }

        if (targetUser.SessionId != null)
        {
            throw new UserAlreadyInUseException(userId);
        }
        
        var sensors = SearchSensors(sensorsIds);
        var pointA = ConvertPoint(fromPointType);
        var pointB = ConvertPoint(toPointType);
        
        _pointsRepository.Add(pointA);
        _pointsRepository.Add(pointB);
        
        var session = new Session()
        {
            StartAt = startAt,
            Name = sessionName
        };
        
        _sessionsRepository.Add(session);

        targetUser.SessionId = session.Id;
        session.Points.Add(pointA);
        session.Points.Add(pointB);
        
        foreach (var sensor in sensors) sensor.BindSensorToSession(session);

        _dataProvider.SaveChanges(); 
    }

    private static Point ConvertPoint(PointType point) => new()
    {
        Longitude = point.Longitude,
        Latitude = point.Latitude
    };
}