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
            throw new UserNotFoundException(id);
        }
        
        using var md5 = MD5.Create();
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        
        var bytes = md5.ComputeHash(passwordBytes);
        var passwordHash = BitConverter.ToString(bytes)
            .Replace("-",
                string.Empty)
            .ToLower();

        return user.PasswordMd5Hash == passwordHash;
    }
}