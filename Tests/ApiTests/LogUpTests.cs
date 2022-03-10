using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using TeenControlSystemWeb.Controllers;
using TeenControlSystemWeb.Data.Models;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Types;
using Xunit;

namespace Tests.ApiTests;

public class LogUpTests
{
    [Fact]
    public async Task Registration_Must_LogUp()
    {
        var dataProvider = new Mock<IDataProvider>();
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(x => x["JwtSecret"]).Returns("7536b1812b2fc0ca67a2cfd9466fdf9b");
        dataProvider.Setup(x => x.UsersRepository.GetAll()).Returns(ArraySegment<User>.Empty);

        var authController = new AuthorizationController(dataProvider.Object, configMock.Object)
         {
             ControllerContext = new TestControllerContext()
         };

        authController.HttpContext.Items["User"] = new User()
        {
            Username = "Admin",
            IsAdmin = true
        };
        
        var userLogUpModel = new UserLogUpType()
        {
            Username = "User",
            Password = "123",
            IsAdmin = true
        };

        var authResponse = await authController.LogUp(userLogUpModel);

        Assert.IsType<OkObjectResult>(authResponse);
    }

    [Fact]
    public async Task Registration_Must_Return_BadRequest_User_Already_Exists_With_Context_Username()
    {
        var dataProvider = new Mock<IDataProvider>();

        dataProvider.Setup(x => x.UsersRepository.GetAll()).Returns(new []
        {
            new User()
            {
                Username = "User"
            }
        });
        
        var authController = new AuthorizationController(dataProvider.Object, new Mock<IConfiguration>().Object)
        {
            ControllerContext = new TestControllerContext()
        };

        authController.HttpContext.Items["User"] = new User()
        {
            Username = "Admin",
            IsAdmin = true
        };

        var userLogUpModel = new UserLogUpType()
        {
            Username = "User",
            Password = "123",
            IsAdmin = true
        };

        var response = await authController.LogUp(userLogUpModel);

        Assert.IsType<BadRequestObjectResult>(response);
    }

    [Fact]
    public async Task Registration_Must_Return_Unauthorized_Invoker_Not_Admin()
    {
        var dataProvider = new Mock<IDataProvider>();

        var authController = new AuthorizationController(dataProvider.Object, new Mock<IConfiguration>().Object)
        {
            ControllerContext = new TestControllerContext()
        };

        authController.HttpContext.Items["User"] = new User()
        {
            Username = "Admin",
            IsAdmin = false
        };
        var userLogUpModel = new UserLogUpType();

        var response = await authController.LogUp(userLogUpModel);

        Assert.IsType<UnauthorizedObjectResult>(response);
    }
}