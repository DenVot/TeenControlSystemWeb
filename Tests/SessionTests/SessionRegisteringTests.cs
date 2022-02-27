using System;
using System.Threading.Tasks;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions.Sensor;
using TeenControlSystemWeb.Exceptions.Session;
using TeenControlSystemWeb.Exceptions.User;
using TeenControlSystemWeb.Providers;
using TeenControlSystemWeb.Types;
using Xunit;

namespace Tests.SessionTests;

public class SessionRegisteringTests
{
    private readonly IDataProvider _testDataProvider = TestDataProvider.Provide();
    private readonly long[] _sensorsIds = {0, 1, 2};
    private const string SessionName = "Session";
    private readonly PointType _fromPoint = new(0, 0);
    private readonly PointType _toPoint = new(10, 10);
    private readonly DateTime _startAt = new(2022, 2, 22);
    
    [Fact]
    public async Task RegisterSession_Must_Register()
    {
        const long userId = 0;
        var sessionProvider = new SessionProvider(_testDataProvider);
        
        await sessionProvider.RegisterSessionAsync(userId,
            SessionName,
            _startAt,
            _sensorsIds,
            _fromPoint,
            _toPoint);
    }

    [Fact]
    public async Task RegisterSession_Must_Throw_Exception_No_User_Found()
    {
        const long userId = 11;
        var sessionProvider = new SessionProvider(_testDataProvider);
        
        await Assert.ThrowsAsync<UserNotFoundException>(() => sessionProvider.RegisterSessionAsync(userId,
            SessionName,
            _startAt,
            _sensorsIds,
            _fromPoint,
            _toPoint));
    }

    [Fact]
    public async Task RegisterSession_Must_Throw_Exception_No_Sensor_Found()
    {
        const long userId = 0;
        var sessionProvider = new SessionProvider(_testDataProvider);
        
        await Assert.ThrowsAsync<SensorNotFoundException>(() => sessionProvider.RegisterSessionAsync(userId,
            SessionName,
            _startAt,
            new[] {0L, 11},
            _fromPoint,
            _toPoint));
    }
    
    [Fact]
    public async Task RegisterSession_Must_Throw_User_Already_In_Use()
    {
        const long userId = 0;
        var sessionProvider = new SessionProvider(_testDataProvider);

        await sessionProvider.RegisterSessionAsync(userId,
            SessionName,
            _startAt,
            _sensorsIds,
            _fromPoint,
            _toPoint);
        
        await Assert.ThrowsAsync<UserAlreadyInUseException>(() => sessionProvider.RegisterSessionAsync(userId,
            SessionName,
            _startAt,
            new []{9L},
            _fromPoint,
            _toPoint));
    }

    [Fact]
    public async Task RegisterSession_Must_Throw_Sensor_Already_In_Use()
    {
        var sessionProvider = new SessionProvider(_testDataProvider);

        await sessionProvider.RegisterSessionAsync(0,
            SessionName,
            _startAt,
            _sensorsIds,
            _fromPoint,
            _toPoint);
        
        await Assert.ThrowsAsync<SensorAlreadyInUseException>(() => sessionProvider.RegisterSessionAsync(1,
            SessionName,
            _startAt,
            _sensorsIds,
            _fromPoint,
            _toPoint));
    }
}