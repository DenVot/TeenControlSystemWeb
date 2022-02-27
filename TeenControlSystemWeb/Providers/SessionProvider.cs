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
    public async Task RegisterSessionAsync(long userId,
        string sessionName,
        DateTime startAt,
        IEnumerable<long> sensorsIds,
        PointType fromPointType,
        PointType toPointType)
    {
        var targetUser = await _usersRepository.FindAsync(userId);

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
        
        await _pointsRepository.AddAsync(pointA);
        await _pointsRepository.AddAsync(pointB);
        
        var session = new Session()
        {
            StartAt = startAt,
            Name = sessionName
        };
        
        await _sessionsRepository.AddAsync(session);

        targetUser.SessionId = session.Id;
        session.Points.Add(pointA);
        session.Points.Add(pointB);
        
        await foreach (var sensor in sensors) sensor!.BindSensorToSession(session);

        await _dataProvider.SaveChangesAsync(); 
    }

    /// <summary>
    /// Начинает сессиию
    /// </summary>
    /// <param name="sessionId">Id сессии</param>
    /// <exception cref="SessionNotFoundException">Вызывается, если сессия не найдена</exception>
    /// <exception cref="SessionAlreadyStartedException">Вызывается, если сессиия уже начата</exception>
    public async Task StartSessionAsync(long sessionId)
    {
        var session = await _sessionsRepository.FindAsync(sessionId);

        if (session == null)
        {
            throw new SessionNotFoundException(sessionId);
        }

        if (session.StartedAt != null)
        {
            throw new SessionAlreadyStartedException(sessionId);
        }
        
        session.StartedAt = DateTime.Now;
        
        await _dataProvider.SaveChangesAsync();
    }

    public void EndSession(long sessionId)
    {
        var session = _sessionsRepository.FindAsync(sessionId);

        if (session == null)
        {
            throw new SessionNotFoundException(sessionId);
        }
        
        
    }
    
    private async IAsyncEnumerable<Sensor?> SearchSensors(IEnumerable<long> ids)
    {
        foreach (var id in ids)
        {
            var sensor = await _sensorsRepository.FindAsync(id);

            if (sensor == null)
            {
                throw new SensorNotFoundException(id);
            }

            yield return sensor;
        }
    }

    private static Point ConvertPoint(PointType point) => new()
    {
        Longitude = point.Longitude,
        Latitude = point.Latitude
    };
}