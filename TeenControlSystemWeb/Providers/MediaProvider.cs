using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions.Media;

namespace TeenControlSystemWeb.Providers;

public class MediaProvider
{
    private readonly IDataProvider _dataProviderObject;

    public MediaProvider(IDataProvider dataProviderObject)
    {
        _dataProviderObject = dataProviderObject;
    }

    public async Task<Stream> GetUserAvatarAsync(long id)
    {
        var avatar = await _dataProviderObject.DefaultAvatarsRepository.FindAsync(id);

        if (avatar == null)
        {
            throw new MediaNotFoundException();
        }

        var avatarBytes = avatar.Avatar;

        return new MemoryStream(avatarBytes);
    }
}