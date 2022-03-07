using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using TeenControlSystemWeb.Data.Models;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions.Sensor;
using TeenControlSystemWeb.Exceptions.User;
using TeenControlSystemWeb.Helpers;
using Xunit;

namespace Tests.SessionTests;

public class SessionEditingTests
{
    [Fact]
    public async Task EditSessionTest_Must_Edit_Nothing()
    {
        var dataProviderMock = new Mock<IDataProvider>();
        var testSession = new Session()
        {
            Id = 0,
            Name = "Test"
        };
        dataProviderMock.Setup(x => x.SessionsRepository.FindAsync(0L)).ReturnsAsync(testSession);

        var sessionProvider = dataProviderMock.ConfigureSessionProvider();

        await sessionProvider.EditSessionAsync(0L, new SessionDelta());
        
        Assert.True(testSession.Name == "Test");
    }
    
    [Fact]
    public async Task EditSessionTest_Must_Edit_Name()
    {
        var dataProviderMock = new Mock<IDataProvider>();
        var testSession = new Session()
        {
            Id = 0,
            Name = "Test"
        };
        dataProviderMock.Setup(x => x.SessionsRepository.FindAsync(0L)).ReturnsAsync(testSession);

        var sessionProvider = dataProviderMock.ConfigureSessionProvider();

        await sessionProvider.EditSessionAsync(0L, new SessionDelta()
        {
            Name = "Test1"
        });
        
        Assert.Equal("Test1", testSession.Name);
    }

    [Fact]
    public async Task EditSessionTest_Must_Edit_StartAt()
    {
        var dataProviderMock = new Mock<IDataProvider>();
        var toEditDate = DateTime.Now.AddDays(1);
        var testSession = new Session()
        {
            Id = 0,
            Name = "Test",
            StartAt = DateTime.Now
        };
        dataProviderMock.Setup(x => x.SessionsRepository.FindAsync(0L)).ReturnsAsync(testSession);

        var sessionProvider = dataProviderMock.ConfigureSessionProvider();

        await sessionProvider.EditSessionAsync(0L, new SessionDelta()
        {
            StartAt = toEditDate
        });
        
        Assert.Equal(toEditDate, testSession.StartAt);
    }

    [Fact]
    public async Task EditSessionTest_Must_Throw_Exception_Invalid_Date()
    {
        var dataProviderMock = new Mock<IDataProvider>();
        var toEditDate = new DateTime(1970, 1, 1);
        var testSession = new Session()
        {
            Id = 0,
            Name = "Test",
            StartAt = DateTime.Now
        };
        dataProviderMock.Setup(x => x.SessionsRepository.FindAsync(0L)).ReturnsAsync(testSession);

        var sessionProvider = dataProviderMock.ConfigureSessionProvider();

        await Assert.ThrowsAsync<InvalidOperationException>(() => sessionProvider.EditSessionAsync(0L,
            new SessionDelta()
            {
                StartAt = toEditDate
            }));
    }

    [Fact]
    public async Task EditSessionTest_Must_Edit_Owner()
    {
        var dataProviderMock = new Mock<IDataProvider>();
        
        var testUserContextOwner = new User()
        {
            Id = 0
        };

        var replaceUserOwner = new User()
        {
            Id = 1
        };
        
        var testSession = new Session()
        {
            Id = 0,
            Owner = testUserContextOwner
        };

        dataProviderMock.Setup(x => x.SessionsRepository.FindAsync(0L)).ReturnsAsync(testSession);
        dataProviderMock.Setup(x => x.UsersRepository.FindAsync(0L)).ReturnsAsync(testUserContextOwner);
        dataProviderMock.Setup(x => x.UsersRepository.FindAsync(1L)).ReturnsAsync(replaceUserOwner);

        var sessionProvider = dataProviderMock.ConfigureSessionProvider();

        await sessionProvider.EditSessionAsync(0L, new SessionDelta()
        {
            OwnerId = 1L
        });
        
        Assert.Equal(1L, testSession.Owner.Id);
    }

    [Fact]
    public async Task EditSessionTest_Must_Throw_Exception_User_Not_Found()
    {
        var dataProviderMock = new Mock<IDataProvider>();
        
        var testUserContextOwner = new User()
        {
            Id = 0
        };

        var testSession = new Session()
        {
            Id = 0,
            Owner = testUserContextOwner
        };

        dataProviderMock.Setup(x => x.SessionsRepository.FindAsync(0L)).ReturnsAsync(testSession);
        dataProviderMock.Setup(x => x.UsersRepository.FindAsync(0L)).ReturnsAsync(testUserContextOwner);
        dataProviderMock.Setup(x => x.UsersRepository.FindAsync(1L)).ReturnsAsync((User?)null);

        var sessionProvider = dataProviderMock.ConfigureSessionProvider();

        await Assert.ThrowsAsync<UserNotFoundException>(() => sessionProvider.EditSessionAsync(0L, new SessionDelta()
        {
            OwnerId = 1L
        }));
    }

    [Fact]
    public async Task EditSessionTest_Must_Add_Sensors()
    {
        var dataProviderMock = new Mock<IDataProvider>();

        var testSession = new Session()
        {
            Id = 0,
            Sensors = new List<Sensor>()
        };

        var testSensor = new Sensor()
        {
            Id = 0
        };

        var sensorsToAdd = new[] {0L};

        dataProviderMock.Setup(x => x.SessionsRepository.FindAsync(0L)).ReturnsAsync(testSession);
        dataProviderMock.Setup(x => x.SensorsRepository.FindAsync(0L)).ReturnsAsync(testSensor);

        var sessionProvider = dataProviderMock.ConfigureSessionProvider();

        await sessionProvider.EditSessionAsync(0L, new SessionDelta()
        {
            SensorsToAdd = sensorsToAdd
        });
        
        Assert.Equal(1, testSession.Sensors.Count);
    }

    [Fact]
    public async Task EditSessionTest_Must_Throw_Exception_Sensor_Not_Found_While_Adding()
    {
        var dataProviderMock = new Mock<IDataProvider>();

        var testSession = new Session()
        {
            Id = 0,
            Sensors = new List<Sensor>()
        };

        var sensorsToAdd = new[] {0L};

        dataProviderMock.Setup(x => x.SessionsRepository.FindAsync(0L)).ReturnsAsync(testSession);
        dataProviderMock.Setup(x => x.SensorsRepository.FindAsync(0L)).ReturnsAsync((Sensor?)null);

        var sessionProvider = dataProviderMock.ConfigureSessionProvider();

        await Assert.ThrowsAsync<SensorNotFoundException>(() => sessionProvider.EditSessionAsync(0L, new SessionDelta()
        {
            SensorsToAdd = sensorsToAdd
        }));
    }

    [Fact]
    public async Task EditSessionTest_Must_Remove_Sensor()
    {
        var dataProviderMock = new Mock<IDataProvider>();

        var testSensor = new Sensor()
        {
            Id = 0
        };
        
        var testSession = new Session()
        {
            Id = 0,
            Sensors = new List<Sensor>()
            {
                testSensor
            }
        };
        
        var sensorsToRemove = new[] {0L};

        dataProviderMock.Setup(x => x.SessionsRepository.FindAsync(0L)).ReturnsAsync(testSession);
        dataProviderMock.Setup(x => x.SensorsRepository.FindAsync(0L)).ReturnsAsync(testSensor);

        var sessionProvider = dataProviderMock.ConfigureSessionProvider();

        await sessionProvider.EditSessionAsync(0L, new SessionDelta()
        {
            SensorsToRemove = sensorsToRemove
        });
        
        Assert.Equal(0, testSession.Sensors.Count);
    }
}