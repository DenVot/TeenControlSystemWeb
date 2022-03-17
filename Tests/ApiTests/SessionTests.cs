using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TeenControlSystemWeb.Controllers;
using TeenControlSystemWeb.Data.Models;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Types;
using Xunit;

namespace Tests.ApiTests;

public class SessionTests
{
    [Fact]
    public async Task CreateSessionTest_Must_Create()
    {
        var dataProviderMock = new Mock<IDataProvider>();
        var sessionProvider = dataProviderMock.ConfigureSessionProvider();
        var testUser = new User()
        {
            Id = 0
        };
            
        var sessionController = new SessionsController(sessionProvider)
        {
            ControllerContext = new TestControllerContext()
        };

        sessionController.HttpContext.Items["User"] = testUser;
        dataProviderMock.Setup(x => x.UsersRepository.FindAsync(0L)).ReturnsAsync(testUser);
        dataProviderMock.Setup(x => x.SensorsRepository.FindAsync(0L)).ReturnsAsync(new Sensor()
        {
            Id = 0
        });
        
        var response = await sessionController.RegisterSession(new RegisterSessionType()
        {
            Name = "Test",
            OwnerId = 0,
            StartAt = DateTime.Now,
            SensorsIds = new [] { 0L },
            FromPoint = new PointType(0, 0),
            ToPoint = new PointType(1, 1)
        });
        
        Assert.IsType<OkObjectResult>(response);
    }
}