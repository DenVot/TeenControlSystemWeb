using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions.User;
using TeenControlSystemWeb.Helpers;

namespace TeenControlSystemWeb.Services;

public class AuthorizationService
{
    private readonly IDataProvider _dataProvider;
    private readonly PasswordComparator _passwordComparator;

    public AuthorizationService(IDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
        _passwordComparator = new PasswordComparator(dataProvider);
    }

    public async Task LogUpAsync(UserLogUpType logUpType)
    {
        using var hasher = new Md5Hasher();
        var allUsers = _dataProvider.UsersRepository.GetAll();

        if (allUsers.Any(x => x.Username == logUpType.Username))
        {
            throw new UserAlreadyExistsWithContextUsernameException();
        }

        var user = new User()
        {
            Username = logUpType.Username,
            IsAdmin = logUpType.IsAdmin,
            PasswordMd5Hash = hasher.HashString(logUpType.Password)
        };

        await _dataProvider.UsersRepository.AddAsync(user);
    }
}