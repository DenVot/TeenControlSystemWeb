using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using TeenControlSystemWeb.Controllers;
using TeenControlSystemWeb.Data.Models;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Types;
using Xunit;

namespace Tests.ApiTests;

public class LoginTests
{
    [Fact]
    public void TestLogin_Must_Login()
    {
        var dataProviderMock = new Mock<IDataProvider>();
        var configMock = new Mock<IConfiguration>();
        
        var loginObj = new LoginType()
        {
            Username = "User",
            Password = "123"
        };
        
        dataProviderMock.Setup(x => x.UsersRepository.GetAll()).Returns(new[]
        {
            new User()
            {
                Id = 0,
                Username = "User",
                PasswordMd5Hash = "202cb962ac59075b964b07152d234b70" //123
            }
        });
        
        configMock.Setup(x => x["JwtSecret"]).Returns("7536b1812b2fc0ca67a2cfd9466fdf9b");
        var authController = new AuthorizationController(dataProviderMock.Object, configMock.Object);

        var result = authController.Login(loginObj);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void LoginTest_Must_Throw_Exception_Failed_To_Auth_User_Not_Found()
    {
        var dataProviderMock = new Mock<IDataProvider>();
        var configMock = new Mock<IConfiguration>();
        
        var loginObj = new LoginType()
        {
            Username = "User",
            Password = "123"
        };
        
        dataProviderMock.Setup(x => x.UsersRepository.GetAll()).Returns(new[]
        {
            new User()
            {
                Id = 0,
                Username = "User1",
                PasswordMd5Hash = "202cb962ac59075b964b07152d234b70" //123
            }
        });
        
        configMock.Setup(x => x["JwtSecret"]).Returns("7536b1812b2fc0ca67a2cfd9466fdf9b");
        var authController = new AuthorizationController(dataProviderMock.Object, configMock.Object);

        var result = authController.Login(loginObj);

        Assert.IsType<UnauthorizedObjectResult>(result);
    }
    
    [Fact]
    public void LoginTest_Must_Throw_Exception_Failed_To_Auth_User_Incorrect_Password()
    {
        var dataProviderMock = new Mock<IDataProvider>();
        var configMock = new Mock<IConfiguration>();
        
        var loginObj = new LoginType()
        {
            Username = "User",
            Password = "123"
        };
        
        dataProviderMock.Setup(x => x.UsersRepository.GetAll()).Returns(new[]
        {
            new User()
            {
                Id = 0,
                Username = "User",
                PasswordMd5Hash = "202cb962ac59075b964b07152d234b71" //123
            }
        });
        
        configMock.Setup(x => x["JwtSecret"]).Returns("7536b1812b2fc0ca67a2cfd9466fdf9b");
        var authController = new AuthorizationController(dataProviderMock.Object, configMock.Object);

        var result = authController.Login(loginObj);

        Assert.IsType<UnauthorizedObjectResult>(result);
    }
}