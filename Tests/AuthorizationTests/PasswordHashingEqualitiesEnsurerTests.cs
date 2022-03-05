using System.Threading.Tasks;
using Moq;
using TeenControlSystemWeb.Data.Models;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions.User;
using Xunit;

namespace Tests.AuthorizationTests;

public class PasswordHashingEqualitiesEnsurerTests
{
    [Fact]
    public async Task TestMd5Hash_Equality()
    {
        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.UsersRepository.FindAsync(0L)).ReturnsAsync(new User()
        {
            Id = 0,
            PasswordMd5Hash = "202cb962ac59075b964b07152d234b70"
        });
        
        var passwordComparator = dataProviderMock.ConfigurePasswordComparator();
        
        Assert.True(await passwordComparator.IsPasswordValidAsync(0, "123"));
    }

    [Fact]
    public async Task TestMd5Hash_Not_Equality()
    {
        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.UsersRepository.FindAsync(0L)).ReturnsAsync(new User()
        {
            Id = 0,
            PasswordMd5Hash = "202cb962ac59075b964b07152d234b70"
        });
        
        var passwordComparator = dataProviderMock.ConfigurePasswordComparator();
        
        Assert.False(await passwordComparator.IsPasswordValidAsync(0, "12"));
    }
    
    [Fact]
    public async Task TestMd5Hash_Must_Throw_User_Not_Found()
    {
        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.UsersRepository.FindAsync(0L)).ReturnsAsync((User?)null);
        
        var passwordComparator = dataProviderMock.ConfigurePasswordComparator();
        
        await Assert.ThrowsAsync<UserNotFoundException>(() => passwordComparator.IsPasswordValidAsync(0L, "123"));
    }
}