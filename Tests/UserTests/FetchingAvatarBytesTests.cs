using System.Threading.Tasks;
using Moq;
using TeenControlSystemWeb.Data.Models;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Providers;
using Xunit;

namespace Tests.UserTests;

public class FetchingAvatarBytesTests
{
    [Fact]
    public async Task FetchAvatarBytes_Must_Fetch()
    {
        byte[] avatarBytes = {1, 2, 3};
        var dataProvider = new Mock<IDataProvider>();

        dataProvider.Setup(x => x.DefaultAvatarsRepository.FindAsync(0L)).ReturnsAsync(new DefaultAvatar()
        {
            Id = 0,
            Avatar = avatarBytes
        });

        var mediaProvider = new MediaProvider(dataProvider.Object);
        var bytesStream = await mediaProvider.GetUserAvatarAsync(0L);

        var buffer = new byte[3];
        
        await bytesStream.ReadAsync(buffer, 0, 3);
        
        Assert.Equal(avatarBytes, buffer);
    }
}