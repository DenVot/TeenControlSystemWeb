namespace TeenControlSystemWeb.Exceptions;

public class UserNotFoundException : SystemException
{
    public UserNotFoundException(long id) : base("Can't find user with following id:" + id)
    {
        
    }
}