namespace TeenControlSystemWeb.Exceptions.Session;

public class SessionNotStartedException : SystemException
{
    public SessionNotStartedException(long id) : base("Session with the following id wasn't started")
    {
        
    }
}