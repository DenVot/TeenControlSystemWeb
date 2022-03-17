using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions.Sensor;
using TeenControlSystemWeb.Exceptions.Session;
using TeenControlSystemWeb.Exceptions.User;
using TeenControlSystemWeb.Extensions;
using TeenControlSystemWeb.Helpers;

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
            throw new UserNotFoundException();
        }

        if (targetUser.SessionId != null)
        {
            throw new UserAlreadyInUseException();
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

        targetUser.Session = session;
        session.Points.Add(pointA);
        session.Points.Add(pointB);

        await foreach (var sensor in sensors)
        {
            sensor!.BindSensorToSession(session);
            sensor!.Online = false;
        }

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
            throw new SessionAlreadyStartedException();
        }
        
        session.StartedAt = DateTime.Now;
        
        await _dataProvider.SaveChangesAsync();
    }

    /// <summary>
    /// Заканчивает сессию
    /// </summary>
    /// <param name="sessionId">Id сесии</param>
    /// <exception cref="SessionNotFoundException">Вызывается, если сессия не найдена</exception>
    /// <exception cref="SessionNotStartedException">Вызывается, если сессия еще не начата</exception>
    public async Task EndSessionAsync(long sessionId)
    {
        var session = await _sessionsRepository.FindAsync(sessionId);

        if (session == null)
        {
            throw new SessionNotFoundException(sessionId);
        }

        if (session.StartedAt == null)
        {
            throw new SessionNotStartedException();
        }
        
        session.EndedAt = DateTime.Now;

        var userLinkedWithSession = session.Owner;

        userLinkedWithSession.Session = null;

        await _dataProvider.SaveChangesAsync();
    }
    
    /// <summary>
    /// Изменяет данные сессии, которые можно изменить
    /// </summary>
    /// <param name="target">Id сессии</param>
    /// <param name="delta">Объект репрезентирующий разницу между текущей сессией и желаемой</param>
    /// <exception cref="SessionNotFoundException">Вызывается, если сессия не найдена</exception>
    /// <exception cref="UserNotFoundException">Вызывается, если новый ответственный не найден по его Id</exception>
    /// <exception cref="InvalidOperationException">Вызывается, если подана некорректная дата</exception>
    /// <exception cref="SensorNotFoundException">Вызывается, если датчик, который нужно привязать/отвязать, не найден</exception>
    public async Task EditSessionAsync(long target, SessionDelta delta)
    {
        var targetSession = await _sessionsRepository.FindAsync(target);

        if (targetSession == null)
        {
            throw new SessionNotFoundException(target);
        }

        var linkedSensors = targetSession.Sensors.Select(x => x.Id).ToArray();
        
        if (delta.Name != null)
        {
            targetSession.Name = delta.Name;
        }

        if (delta.OwnerId != null && delta.OwnerId != targetSession.Owner.Id)
        {
            var targetUser = await _usersRepository.FindAsync(delta.OwnerId);

            if (targetUser == null)
            {
                throw new UserNotFoundException();
            }

            targetSession.Owner = targetUser;
        }

        if (delta.StartAt != null)
        {
            var now = DateTime.Now;

            if (delta.StartAt < now)
            {
                throw new InvalidOperationException("Измененная дата не должна быть в прошлом");
            }

            targetSession.StartAt = delta.StartAt.Value;
        }

        if (delta.SensorsToAdd != null)
        {
            foreach (var sensorId in delta.SensorsToAdd)
            {
                if (linkedSensors.Any(x => x == sensorId))
                {
                    continue;
                }
                
                var sensor = await _sensorsRepository.FindAsync(sensorId);
                
                if (sensor == null)
                {
                    throw new SensorNotFoundException();
                }
                
                targetSession.Sensors.Add(sensor);
            }
        }

        if (delta.SensorsToRemove != null)
        {
            foreach (var sensorId in delta.SensorsToRemove)
            {
                if (linkedSensors.All(x => x != sensorId))
                {
                    continue;
                }
                
                var sensor = await _sensorsRepository.FindAsync(sensorId);
                
                if (sensor == null)
                {
                    throw new SensorNotFoundException();
                }

                targetSession.Sensors.Remove(sensor);
            }
        }

        await _dataProvider.SaveChangesAsync();
    }

    private async IAsyncEnumerable<Sensor?> SearchSensors(IEnumerable<long> ids) //Поиск маячков по идентификаторам
    {
        foreach (var id in ids)
        {
            var sensor = await _sensorsRepository.FindAsync(id);

            if (sensor == null)
            {
                throw new SensorNotFoundException();
            }

            yield return sensor;
        }
    }

    private static Point ConvertPoint(PointType point) => new()
    {
        Longitude = point.Longitude,
        Latitude = point.Latitude
    };

    /// <summary>
    /// Обновляет сессию
    /// </summary>
    /// <param name="id">Иднетификатор сессии</param>
    /// <param name="snapshot">Снапшот сессии</param>
    /// <exception cref="SessionNotFoundException">Вызывается, если сессия не найдена</exception>
    /// <exception cref="KeyNotFoundException">Вызывается, если не найдено обновления для одного из маячков</exception>
    public async Task UpdateSessionStateAsync(long id, SessionSnapshot snapshot)
    {
        var session = await _sessionsRepository.FindAsync(id);

        if (session == null)
        {
            throw new SessionNotFoundException(id);
        }
        
        foreach (var sensor in session.Sensors)
        {
            var sensorId = sensor.Id;
            
            if (!snapshot.Sensors.ContainsKey(sensorId))
            {
                throw new KeyNotFoundException("Не найдено обновление для маячка с ID " + sensorId);
            }

            sensor.Online = snapshot.Sensors[sensorId];
        }

        var point = new Point()
        {
            Longitude = snapshot.Point.Longitude,
            Latitude = snapshot.Point.Latitude
        };

        await _pointsRepository.AddAsync(point);
        
        session.Points.Add(point);

        await _dataProvider.SaveChangesAsync();
    }
}