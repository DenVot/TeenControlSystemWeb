using System;
using Moq;
using TeenControlSystemWeb.Data.Models;
using TeenControlSystemWeb.Data.Repositories;
using Xunit;

namespace Tests.SessionTests;

public class ActiveSessionGetter
{
    [Fact]
    public void GetAllActiveSessions_Must_Get()
    {
        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.SessionsRepository.GetAll()).Returns(() => new[]
        {
            new Session() //Active
            {
                Id = 0,
                StartedAt = DateTime.Now
            },
            new Session() //Not active
            {
                Id = 1
            },
            new Session() //Not active
            {
                Id = 2,
                StartAt = DateTime.Now.AddHours(-2),
                EndedAt = DateTime.Now
            }
        });

        var sessionProvider = dataProviderMock.ConfigureSessionProvider();

        var sessions = sessionProvider.GetActiveSessions();
        
        Assert.Single(sessions);
    }
}