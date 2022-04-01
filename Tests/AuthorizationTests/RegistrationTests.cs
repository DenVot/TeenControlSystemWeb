using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using TeenControlSystemWeb.Data.Models;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions.User;
using TeenControlSystemWeb.Types;
using Xunit;

namespace Tests.AuthorizationTests;

public class RegistrationTests
{
    [Fact]
    public async Task Registration_Must_LogUp()
    {
        var dataProvider = new Mock<IDataProvider>();
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(x => x["JwtSecret"]).Returns("7536b1812b2fc0ca67a2cfd9466fdf9b");
        dataProvider.Setup(x => x.UsersRepository.GetAll()).Returns(ArraySegment<User>.Empty);
        dataProvider.Setup(x => x.DefaultAvatarsRepository.GetAll()).Returns(() => new List<DefaultAvatar>()
        {
            new()
            {
                Id = 0
            },
            new()
            {
                Id = 1
            }
        });
        
        var authService = dataProvider.ConfigureAuthorizationService(configMock.Object);

        var userLogUpModel = new UserLogUpType()
        {
            Username = "User",
            Password = "123",
            IsAdmin = true
        };

        var authResponse = await authService.LogUpAsync(userLogUpModel);
        
        Assert.NotNull(authResponse.User);
        Assert.NotNull(authResponse.Token);
        Assert.True(authResponse.User.AvatarId == 0 || authResponse.User.AvatarId == 1);
    }

    [Fact]
    public async Task Registration_Must_Throw_Exception_User_Already_Exists_With_Context_Username()
    {
        var dataProvider = new Mock<IDataProvider>();

        dataProvider.Setup(x => x.UsersRepository.GetAll()).Returns(new []
        {
            new User()
            {
                Username = "User"
            }
        });
        dataProvider.Setup(x => x.DefaultAvatarsRepository.GetAll()).Returns(() => new List<DefaultAvatar>()
        {
            new()
            {
                Id = 0
            },
            new()
            {
                Id = 1
            }
        });
        
        var authService = dataProvider.ConfigureAuthorizationService();

        var userLogUpModel = new UserLogUpType()
        {
            Username = "User",
            Password = "123",
            IsAdmin = true
        };

        await Assert.ThrowsAsync<UserAlreadyExistsWithContextUsernameException>(() => authService.LogUpAsync(userLogUpModel));
    }
}