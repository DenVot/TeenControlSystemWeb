using System;
using System.Collections.Generic;
using System.Linq;
using TeenControlSystemWeb.Data.Models;
using TeenControlSystemWeb.Data.Repositories;

namespace Tests;

public class TestDataProvider : IDataProvider
{
    public TestDataProvider(IEnumerable<Point>? points = null,
        IEnumerable<Sensor>? sensors = null,
        IEnumerable<Session>? sessions = null,
        IEnumerable<User>? users = null,
        IEnumerable<UserAuthorizationToken>? tokens = null)
    {
        PointsRepository = new TestRepository<Point>(x => x.Id, points);
        SensorsRepository = new TestRepository<Sensor>(x => x.Id, sensors);
        SessionsRepository = new TestRepository<Session>(x => x.Id, sessions);
        UsersRepository = new TestRepository<User>(x => x.Id, users);
        UserAuthorizationTokensRepository = new TestRepository<UserAuthorizationToken>(x => x.Id, tokens);
    }
    
    public IRepository<Point> PointsRepository { get; }
    public IRepository<Sensor> SensorsRepository { get; }
    public IRepository<Session> SessionsRepository { get; }
    public IRepository<User> UsersRepository { get; }
    public IRepository<UserAuthorizationToken> UserAuthorizationTokensRepository { get; }
    
    public void SaveChanges()
    {
        
    }

    public static IDataProvider Provide(int count = 10)
    {
        var now = DateTime.Now;
        var rand = new Random();

        var points = new Point[count];
        var sensors = new Sensor[count];
        var sessions = new Session[count];
        var users = new User[count];

        string GenGuid() => new Guid().ToString();
        
        //Filling points
        for (int i = 0; i < count; i++)
        {
            var time = now.AddHours(i);

            points[i] = new Point()
            {
                Id = i,
                AddedAt = time,
                Latitude = rand.NextDouble(),
                Longitude = rand.NextDouble(),
                SessionId = 0
            };

            sessions[i] = new Session()
            {
                Id = i,
                FromId = 0,
                ToId = 1,
                Name = GenGuid(),
                StartAt = time,
                OwnerId = 0
            };

            sensors[i] = new Sensor()
            {
                Id = i,
                Mac = GenGuid(),
                Name = GenGuid()
            };

            users[i] = new User()
            {
                Id = i,
                IsAdmin = false,
                PasswordMd5Hash = GenGuid(),
                Username = GenGuid()
            };
        }

        return new TestDataProvider(points, sensors, sessions, users);
    }
}

public class TestRepository<T> : IRepository<T> where T : class
{
    private readonly Func<T, long> _getId;
    private readonly ICollection<T> _array;

    public TestRepository(Func<T, long> getId, IEnumerable<T>? array)
    {
        _getId = getId;
        _array = array?.ToList() ?? new List<T>();
    }

    public T? Find(object id) => _array.FirstOrDefault(x => _getId(x) == (int) id);

    public void Add(T obj)
    {
        _array.Add(obj);
    }

    public void AddRange(IEnumerable<T> objs)
    {
        foreach (var obj in objs)
        {
            Add(obj);
        }
    }

    public void Remove(T target)
    {
        _array.Remove(target);
    }

    public void RemoveRange(IEnumerable<T> targets)
    {
        foreach (var target in targets)
        {
            Remove(target);
        }
    }
}