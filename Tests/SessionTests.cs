using System;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions;
using TeenControlSystemWeb.Providers;
using TeenControlSystemWeb.Types;
using Xunit;

namespace Tests;

public class SessionTests
{
    private readonly IDataProvider _testDataProvider = TestDataProvider.Provide();

    [Fact]
    public void RegisterSession_Must_Register()
    {
        var userId = 0;
        var sensorsIds = new[] {0, 1, 2};
        var fromPoint = new PointType(0, 0);
        var toPoint = new PointType(10, 10);
        

        var sessionName = "Session";
        var startAt = new DateTime(2022, 2, 22);
        
        var sessionProvider = new SessionProvider(_testDataProvider);
        sessionProvider.RegisterSession(userId,
            sessionName,
            startAt,
            sensorsIds,
            fromPoint,
            toPoint);
    }

    [Fact]
    public void RegisterSession_Must_Throw_Exception_No_User_Found()
    {
        var userId = 11;
        var sensorsIds = new[] {0, 1, 2};
        var fromPoint = new PointType(0, 0);
        var toPoint = new PointType(10, 10);
        

        var sessionName = "Session";
        var startAt = new DateTime(2022, 2, 22);
        
        var sessionProvider = new SessionProvider(_testDataProvider);
        
        Assert.Throws<UserNotFoundException>(() => sessionProvider.RegisterSession(userId,
            sessionName,
            startAt,
            sensorsIds,
            fromPoint,
            toPoint));
    }
}