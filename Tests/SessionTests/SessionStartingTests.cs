using System;
using System.Threading.Tasks;
using Moq;
using TeenControlSystemWeb.Data.Models;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions.Session;
using TeenControlSystemWeb.Providers;
using Xunit;

namespace Tests.SessionTests;

public class SessionStartingTests
{
    [Fact]
    public async Task StartSessionTest_Must_Start()
    {
        const long sessionId = 0;
        var dataProviderMock = new Mock<IDataProvider>(MockBehavior.Loose);

        dataProviderMock.Setup(x => x.SessionsRepository.FindAsync(sessionId)).ReturnsAsync(new Session()
        {
            Id = 0,
            OwnerId = 0,
            Owner = new User()
            {
                Id = 0
            }
        });
        
        var sessionsProvider = new SessionProvider(dataProviderMock.Object);

        await sessionsProvider.StartSessionAsync(sessionId);
    }

    [Fact]
    public async Task StartSessionTest_Must_Throw_Exception_Session_Not_Found()
    {
        const long sessionId = 10;
        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.SessionsRepository.FindAsync(sessionId))
            .ReturnsAsync((Session?)null);

        var sessionsProvider = dataProviderMock.ConfigureSessionProvider();

        await Assert.ThrowsAsync<SessionNotFoundException>(() =>
            sessionsProvider.StartSessionAsync(sessionId));
    }

    [Fact]
    public async Task StartSessionTest_Must_Throw_Exception_Session_Already_Started()
    {
        const long sessionId = 0;
        var dataProviderMock = new Mock<IDataProvider>();
        
        dataProviderMock.Setup(x => x.SessionsRepository.FindAsync(sessionId)).ReturnsAsync(new Session()
        {
            Id = 0,
            OwnerId = 0,
            Owner = new User()
            {
                Id = 0
            },
            StartedAt = DateTime.Now
        });
        
        var sessionsProvider = dataProviderMock.ConfigureSessionProvider();

        await Assert.ThrowsAsync<SessionAlreadyStartedException>(() => 
            sessionsProvider.StartSessionAsync(sessionId));
    }
}