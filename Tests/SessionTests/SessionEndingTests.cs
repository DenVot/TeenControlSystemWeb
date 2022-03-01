using System;
using System.Threading.Tasks;
using Moq;
using TeenControlSystemWeb.Data.Models;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions.Session;
using TeenControlSystemWeb.Providers;
using Xunit;

namespace Tests.SessionTests;

public class SessionEndingTests
{
    [Fact]
    public async Task EndSessionTest_Must_End()
    {
        const long id = 0;
        var mockDataProvider = new Mock<IDataProvider>();

        mockDataProvider.Setup(x => x.SessionsRepository.FindAsync(id)).ReturnsAsync(new Session()
        {
            Id = 0,
            StartedAt = DateTime.Now,
            Owner = new User()
            {
                Id = 0,
                SessionId = 0
            }
        });
        
        var sessionProvider = mockDataProvider.ConfigureSessionProvider();

        await sessionProvider.EndSessionAsync(id);
    }

    [Fact]
    public async Task EndSession_Must_Throw_Exception_Session_Not_Found()
    {
        var mockDataProvider = new Mock<IDataProvider>();

        mockDataProvider.Setup(x => x.SessionsRepository.FindAsync(0L)).ReturnsAsync((Session?) null);
        
        var sessionProvider = mockDataProvider.ConfigureSessionProvider();

        await Assert.ThrowsAsync<SessionNotFoundException>(() => sessionProvider.EndSessionAsync(0L));
    }
    
    [Fact]
    public async Task EndSession_Must_Throw_Exception_Session_Not_Started()
    {
        const long id = 0;
        var mockDataProvider = new Mock<IDataProvider>();

        mockDataProvider.Setup(x => x.SessionsRepository.FindAsync(id)).ReturnsAsync(new Session()
        {
            Id = 0,
            Owner = new User()
            {
                Id = 0,
                SessionId = 0
            },
            StartedAt = null
        });

        var sessionProvider = mockDataProvider.ConfigureSessionProvider();
        
        await Assert.ThrowsAsync<SessionNotStartedException>(() => sessionProvider.EndSessionAsync(id));
    }
}