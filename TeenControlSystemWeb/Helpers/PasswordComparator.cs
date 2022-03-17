using System.Security.Cryptography;
using System.Text;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions.User;

namespace TeenControlSystemWeb.Helpers;

public class PasswordComparator
{
    private readonly IRepository<User> _usersRepository;

    public PasswordComparator(IDataProvider mockObject)
    {
        _usersRepository = mockObject.UsersRepository;
    }

    public async Task<bool> IsPasswordValidAsync(long id, string password)
    {
        var user = await _usersRepository.FindAsync(id);

        if (user == null)
        {
            throw new UserNotFoundException();
        }

        using var hasher = new Md5Hasher();
        
        return user.PasswordMd5Hash == hasher.HashString(password);
    }
}