namespace TeenControlSystemWeb.Exceptions.User;

public class UserNotFoundException : SystemException
{
    public UserNotFoundException() : base("Пользователь не найден")
    {
        
    }
}