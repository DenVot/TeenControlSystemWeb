namespace TeenControlSystemWeb.Exceptions.User;

public class UserAlreadyInUseException : SystemException
{
    public UserAlreadyInUseException() : base("Пользователь занят")
    {
        
    }
}