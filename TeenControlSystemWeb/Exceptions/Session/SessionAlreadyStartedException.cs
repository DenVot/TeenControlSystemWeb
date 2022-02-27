namespace TeenControlSystemWeb.Exceptions.Session;

public class SessionAlreadyStartedException : SystemException
{
    public SessionAlreadyStartedException(long id) : base("Session with the following id already started: " + id)
    {

    }
}