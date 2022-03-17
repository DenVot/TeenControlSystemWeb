namespace TeenControlSystemWeb.Exceptions.Session;

public class SessionAlreadyStartedException : SystemException
{
    public SessionAlreadyStartedException() : base("Сессия уже начата")
    {

    }
}