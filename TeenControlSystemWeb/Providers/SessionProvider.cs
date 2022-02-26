using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions;
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

    public void RegisterSession(int userId, string sessionName, DateTime startAt, int[] sensorsIds,
        PointType fromPointType,
        PointType toPointType)
    {
        var targetUser = _usersRepository.Find(userId);

        if (targetUser == null)
        {
            throw new UserNotFoundException(userId);
        }
        
        var sensors = from sensorId in sensorsIds
            select _sensorsRepository.Find(sensorId);

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

        session.Owner = targetUser;
        session.Points.Add(pointA);
        session.Points.Add(pointB);
        
        foreach (var sensor in sensors)
        {
            session.Sensors.Add(sensor);
        }
        
        _dataProvider.SaveChanges(); 
    }

    private static Point ConvertPoint(PointType point) => new()
    {
        Longitude = point.Longitude,
        Latitude = point.Latitude
    };
}