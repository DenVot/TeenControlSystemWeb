using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using TeenControlSystemWeb.Data.Models;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions.Session;
using TeenControlSystemWeb.Types;
using Xunit;

namespace Tests.SessionTests;

public class SessionStateUpdateTests
{
    [Fact]
    public async Task UpdateStateTest_Must_Update()
    {
        var sensor = new Sensor()
        {
            Id = 0,
            Online = false
        };
        
        var sessionSnapshot = new SessionSnapshot()
        {
            Sensors = new Dictionary<long, bool>()
            {
                {0, true}
            },
            Point = new PointType()
            {
                Longitude = 0,
                Latitude = 0
            }
        };

        var session = new Session()
        {
            Id = 0,
            Sensors = new[] {sensor},
            Points = new List<Point>(),
            StartAt = DateTime.Now.AddHours(-1),
            StartedAt = DateTime.Now.AddHours(-0.9)
        };

        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.SensorsRepository.FindAsync(0L)).ReturnsAsync(sensor);
        dataProviderMock.Setup(x => x.SessionsRepository.FindAsync(0L)).ReturnsAsync(session);
        dataProviderMock.Setup(x => x.PointsRepository.AddAsync(new Point()));

        var sessionProvider = dataProviderMock.ConfigureSessionProvider();

        await sessionProvider.UpdateSessionStateAsync(0L, sessionSnapshot);
        
        Assert.Equal(1, session.Points.Count);
        Assert.Equal(true, sensor.Online);
    }

    [Fact]
    public async Task UpdateStateTest_Must_Throw_Exception_Session_Not_Found()
    {
        var dataProviderMock = new Mock<IDataProvider>();
        var sessionSnapshot = new SessionSnapshot();
        
        dataProviderMock.Setup(x => x.SessionsRepository.FindAsync(0L)).ReturnsAsync((Session?)null);

        var sessionProvider = dataProviderMock.ConfigureSessionProvider();

        await Assert.ThrowsAsync<SessionNotFoundException>(() => sessionProvider.UpdateSessionStateAsync(0L, sessionSnapshot));
    }
    
    [Fact]
    public async Task UpdateStateTest_Must_Throw_Exception_Update_Not_Found()
    {
        var sensor = new Sensor()
        {
            Id = 0,
            Online = false
        };
        
        var sessionSnapshot = new SessionSnapshot()
        {
            Sensors = new Dictionary<long, bool>()
            {
                
            },
            Point =  new PointType()
            {
                Longitude = 0,
                Latitude = 0
            }
        };

        var session = new Session()
        {
            Id = 0,
            Sensors = new[] {sensor},
            Points = new List<Point>(),
            StartAt = DateTime.Now.AddHours(-1),
            StartedAt = DateTime.Now.AddHours(-0.9)
        };

        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.SensorsRepository.FindAsync(0L)).ReturnsAsync(sensor);
        dataProviderMock.Setup(x => x.SessionsRepository.FindAsync(0L)).ReturnsAsync(session);
        dataProviderMock.Setup(x => x.PointsRepository.AddAsync(new Point()));

        var sessionProvider = dataProviderMock.ConfigureSessionProvider();

        await Assert.ThrowsAsync<KeyNotFoundException>(() => sessionProvider.UpdateSessionStateAsync(0L, sessionSnapshot));
    }
}