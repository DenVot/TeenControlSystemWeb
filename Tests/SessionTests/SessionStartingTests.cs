using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions.Session;
using TeenControlSystemWeb.Providers;
using Xunit;

namespace Tests.SessionTests;

public class SessionStartingTests
{
    private readonly IDataProvider _testDataProvider = TestDataProvider.Provide();

    [Fact]
    public void StartSessionTest_Must_Start()
    {
        const long sessionId = 0;
        var sessionsProvider = new SessionProvider(_testDataProvider);
        
        sessionsProvider.StartSession(sessionId);

        Assert.True(_testDataProvider.SessionsRepository.Find(sessionId)!.StartedAt != null);
    }

    [Fact]
    public void StartSessionTest_Must_Throw_Exception_Session_Not_Found()
    {
        const long sessionId = 10;
        var sessionsProvider = new SessionProvider(_testDataProvider);
        
        Assert.Throws<SessionNotFoundException>(() =>
            sessionsProvider.StartSession(sessionId));
    }

    [Fact]
    public void StartSessionTest_Must_Throw_Exception_Session_Already_Started()
    {
        const long sessionId = 0;
        var sessionsProvider = new SessionProvider(_testDataProvider);
        
        sessionsProvider.StartSession(sessionId);
        
        Assert.Throws<SessionAlreadyStartedException>(() => 
            sessionsProvider.StartSession(sessionId));
    }
}