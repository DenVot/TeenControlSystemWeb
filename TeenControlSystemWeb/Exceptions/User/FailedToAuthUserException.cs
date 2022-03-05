namespace TeenControlSystemWeb.Exceptions.User;

public class FailedToAuthUserException : SystemException
{
    public FailedToAuthUserException(string reason = "Ошибка авторизации") : base(reason)
    {
        
    }
}