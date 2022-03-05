using System;
using System.Threading.Tasks;
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

        dataProvider.Setup(x => x.UsersRepository.GetAll()).Returns(ArraySegment<User>.Empty);
        
        var authService = dataProvider.ConfigureAuthorizationService();

        var userLogUpModel = new UserLogUpType()
        {
            Username = "User",
            Password = "123",
            IsAdmin = true
        };

        await authService.LogUpAsync(userLogUpModel);
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