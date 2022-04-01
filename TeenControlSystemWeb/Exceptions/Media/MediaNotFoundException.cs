namespace TeenControlSystemWeb.Exceptions.Media;

public class MediaNotFoundException : SystemException
{
    public MediaNotFoundException() : base("Медиа по заданному запросу не найдено")
    {
        
    }
}