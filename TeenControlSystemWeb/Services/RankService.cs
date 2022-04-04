using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TeenControlSystemWeb.Data.Repositories;

namespace TeenControlSystemWeb.Services;

/// <summary>
/// Предоставляет систему рангов в виде SHA256 хешей
/// </summary>
public class RankService
{
    private readonly IDataProvider _dataProvider;
    private static byte[][]? _adminsHash;

    private const int Sha256Length = 64;
    
    public RankService(IDataProvider dataProvider)
    {
        _dataProvider = dataProvider;

        if (_adminsHash == null)
        {
            FillMemoryHash(dataProvider.UsersRepository.GetAll());
        }
    }

    /// <summary>
    /// Проверяет, является ли пользователь с <see cref="userId"/> админом.
    /// </summary>
    /// <param name="userId">Индентификатор пользователя</param>
    public bool IsAdmin(long userId)
    {
        if (_adminsHash == null)
        {
            FillMemoryHash();
        }
        
        using var sha256 = SHA256.Create();

        var sha256Bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(userId.ToString()));
        var incorrect = false;
        
        foreach (var bytes in _adminsHash!)
        {
            for (var i = 0; i < bytes.Length; i++)
            {
                if(bytes[i] != sha256Bytes[i]) incorrect = true; break;
            }

            if (!incorrect)
            {
                return true;
            }

            incorrect = false;
        }

        return false;
    }

    /// <summary>
    /// Заполняет хэш таблицу
    /// </summary>
    public void FillMemoryHash()
    {
       FillMemoryHash(_dataProvider.UsersRepository.GetAll());
    }

    public static void FillMemoryHash(IEnumerable<User> usersEnumerable)
    {
        User[] admins;

        if (usersEnumerable is DbSet<User> users)
        {
            admins = users.FromSqlRaw("SELECT * FROM dbo.Users WHERE IsAdmin = 'TRUE'").ToArray();
        }
        else
        {
            admins = usersEnumerable.Where(x => x.IsAdmin).ToArray();
        }

        using var sha256 = SHA256.Create();

        _adminsHash = new byte[admins.Length][];
        
        for (var i = 0; i < admins.Length; i++)
        {
            _adminsHash[i] = new byte[Sha256Length];
            var username = admins[i].Id.ToString();

            var sha256Hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(username));

            _adminsHash[i] = sha256Hash;
        }
    }
}