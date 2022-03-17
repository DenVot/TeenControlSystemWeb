namespace TeenControlSystemWeb.Exceptions.Session;

public class SessionNotFoundException : SystemException
{
    private readonly long _id;

    public SessionNotFoundException(long id) : base("Сессия не найдена")
    {
        _id = id;
    }
}