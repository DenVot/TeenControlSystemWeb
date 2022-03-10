using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions.User;
using TeenControlSystemWeb.Extensions;
using TeenControlSystemWeb.Helpers;
using TeenControlSystemWeb.Responses;

namespace TeenControlSystemWeb.Services;

public class AuthorizationService
{
    private readonly IDataProvider _dataProvider;
    private readonly IConfiguration _configuration;
    private readonly PasswordComparator _passwordComparator;

    public AuthorizationService(IDataProvider dataProvider, IConfiguration configuration)
    {
        _dataProvider = dataProvider;
        _configuration = configuration;
        _passwordComparator = new PasswordComparator(dataProvider);
    }

    /// <summary>
    /// Регистрирует пользователя
    /// </summary>
    /// <param name="logUpType">Данные регистрации</param>
    /// <returns>Возвращает данные пользователя и JWT токен</returns>
    /// <exception cref="UserAlreadyExistsWithContextUsernameException">
    /// Вызывается, если пользователь уже существует с таким именем
    /// </exception>
    public async Task<AuthResponse> LogUpAsync(UserLogUpType logUpType)
    {
        using var hasher = new Md5Hasher();
        var allUsers = _dataProvider.UsersRepository.GetAll().ToArray();

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
        await _dataProvider.SaveChangesAsync();

        return GenerateResponseByUserId(user);
    }

    /// <summary>
    /// Выполняет вход пользователя
    /// </summary>
    /// <param name="loginObj">Данные логинизации</param>
    /// <returns>Возвращает данные пользователя и JWT токен</returns>
    /// <exception cref="FailedToAuthUserException">
    /// Вызывается, если не получилось авторизовать пользователя
    /// </exception>
    public AuthResponse Login(LoginType loginObj)
    {
        using var md5Hasher = new Md5Hasher();
        
        var firstAssociatedUser = _dataProvider.UsersRepository.GetAll().FirstOrDefault(x =>
            x.Username == loginObj.Username && x.PasswordMd5Hash == md5Hasher.HashString(loginObj.Password));

        if (firstAssociatedUser == null)
        {
            throw new FailedToAuthUserException("Неверный логин или пароль");
        }

        return GenerateResponseByUserId(firstAssociatedUser);
    }

    private AuthResponse GenerateResponseByUserId(User user) //Создание ответа с помощью идентификатора пользователя
    {
        var jwtHelper = new JwtHelper();
        var jwtToken = jwtHelper.GenerateJwt(_configuration["JwtSecret"], user.Id);

        return new AuthResponse()
        {
            Token = jwtToken,
            User = user.ConvertToApiType()
        };
    }
}