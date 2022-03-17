namespace TeenControlSystemWeb.Exceptions.User;

public class UserAlreadyExistsWithContextUsernameException : SystemException
{
    public UserAlreadyExistsWithContextUsernameException() : base("Пользователь с таким именем уже существует")
    {
        
    }
}