using System;
using System.Threading.Tasks;
using Moq;
using TeenControlSystemWeb.Data.Models;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions.Sensor;
using TeenControlSystemWeb.Exceptions.User;
using TeenControlSystemWeb.Providers;
using TeenControlSystemWeb.Types;
using Xunit;

namespace Tests.SessionTests;

public class SessionRegisteringTests
{
    private readonly long[] _sensorsIds = {0};
    private const string SessionName = "Session";
    private readonly PointType _fromPoint = new(0, 0);
    private readonly PointType _toPoint = new(10, 10);
    private readonly DateTime _startAt = new(2022, 2, 22);
    
    [Fact]
    public async Task RegisterSession_Must_Register()
    {
        const long userId = 0;
        var dataProviderMock = new Mock<IDataProvider>(MockBehavior.Loose);

        dataProviderMock.Setup(x => x.UsersRepository.FindAsync(userId)).ReturnsAsync(new User()
        {
            Id = 0,
            Session = null
        });

        dataProviderMock.Setup(x => x.SensorsRepository.FindAsync(0L)).ReturnsAsync(new Sensor()
        {
            Id = 0,
            Session = null
        });

        dataProviderMock.Setup(x => x.PointsRepository.AddAsync(null!));
        dataProviderMock.Setup(x => x.SessionsRepository.AddAsync(null!));
        
        var sessionProvider = dataProviderMock.ConfigureSessionProvider();
        
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
        var dataProviderMock = new Mock<IDataProvider>();
        
        dataProviderMock.Setup(x => x.UsersRepository.FindAsync(userId))
            .ReturnsAsync((User?) null);
        
        dataProviderMock.Setup(x => x.SensorsRepository.FindAsync(0)).ReturnsAsync(new Sensor()
        {
            Id = 0,
            Session = null
        });
        
        dataProviderMock.Setup(x => x.PointsRepository.AddAsync(null!));
        dataProviderMock.Setup(x => x.SessionsRepository.AddAsync(null!));
        
        var sessionProvider = dataProviderMock.ConfigureSessionProvider();
        
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
        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.UsersRepository.FindAsync(userId))
            .ReturnsAsync(new User()
            {
                Id = 0,
                Session = null
            });
        
        dataProviderMock.Setup(x => x.SensorsRepository.FindAsync(0)).ReturnsAsync((Sensor?)null);
        dataProviderMock.Setup(x => x.PointsRepository.AddAsync(null!));
        dataProviderMock.Setup(x => x.SessionsRepository.AddAsync(null!));
        
        var sessionProvider = dataProviderMock.ConfigureSessionProvider();
        
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
        
        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.UsersRepository.FindAsync(userId))
            .ReturnsAsync(new User()
            {
                Id = 0,
                SessionId = 0
            });
        
        dataProviderMock.Setup(x => x.SensorsRepository.FindAsync(0)).ReturnsAsync(new Sensor()
        {
            Id = 0,
            Session = null
        });
        
        dataProviderMock.Setup(x => x.PointsRepository.AddAsync(null!));
        dataProviderMock.Setup(x => x.SessionsRepository.AddAsync(null!));
        
        var sessionProvider = dataProviderMock.ConfigureSessionProvider();

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
        var dataProviderMock = new Mock<IDataProvider>();
        
        dataProviderMock.Setup(x => x.UsersRepository.FindAsync(0L))
            .ReturnsAsync(new User()
            {
                Id = 0
            });
        
        dataProviderMock.Setup(x => x.SensorsRepository.FindAsync(0L)).ReturnsAsync(new Sensor()
        {
            Id = 0,
            SessionId = 0
        });
        
        dataProviderMock.Setup(x => x.PointsRepository.AddAsync(null!));
        dataProviderMock.Setup(x => x.SessionsRepository.AddAsync(null!));
        
        var sessionProvider = dataProviderMock.ConfigureSessionProvider();

        await Assert.ThrowsAsync<SensorAlreadyInUseException>(() => sessionProvider.RegisterSessionAsync(0,
            SessionName,
            _startAt,
            _sensorsIds,
            _fromPoint,
            _toPoint));
    }
}