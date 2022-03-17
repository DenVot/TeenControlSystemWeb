namespace TeenControlSystemWeb.Exceptions.Session;

public class SessionNotStartedException : SystemException
{
    public SessionNotStartedException() : base("Сессия не начата")
    {
        
    }
}