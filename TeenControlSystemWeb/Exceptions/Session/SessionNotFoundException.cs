namespace TeenControlSystemWeb.Exceptions.Session;

public class SessionNotFoundException : SystemException
{
    public SessionNotFoundException(long id) : base("Session with following id was not found: " + id)
    {
        
    }
}