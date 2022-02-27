using System.Threading.Tasks;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions.Session;
using TeenControlSystemWeb.Providers;
using Xunit;

namespace Tests.SessionTests;

public class SessionStartingTests
{
    private readonly IDataProvider _testDataProvider = TestDataProvider.Provide();

    [Fact]
    public async Task StartSessionTest_Must_Start()
    {
        const long sessionId = 0;
        var sessionsProvider = new SessionProvider(_testDataProvider);
        
        await sessionsProvider.StartSessionAsync(sessionId);

        Assert.True((await _testDataProvider.SessionsRepository.FindAsync(sessionId))!.StartedAt != null);
    }

    [Fact]
    public async Task StartSessionTest_Must_Throw_Exception_Session_Not_Found()
    {
        const long sessionId = 10;
        var sessionsProvider = new SessionProvider(_testDataProvider);
        
        await Assert.ThrowsAsync<SessionNotFoundException>(() =>
            sessionsProvider.StartSessionAsync(sessionId));
    }

    [Fact]
    public async Task StartSessionTest_Must_Throw_Exception_Session_Already_Started()
    {
        const long sessionId = 0;
        var sessionsProvider = new SessionProvider(_testDataProvider);
        
        await sessionsProvider.StartSessionAsync(sessionId);
        
        await Assert.ThrowsAsync<SessionAlreadyStartedException>(() => 
            sessionsProvider.StartSessionAsync(sessionId));
    }
}