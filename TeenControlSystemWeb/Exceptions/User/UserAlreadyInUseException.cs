namespace TeenControlSystemWeb.Exceptions.User;

public class UserAlreadyInUseException : SystemException
{
    public UserAlreadyInUseException(long id) : base("User with following id is already in use:" + id)
    {
        
    }
}