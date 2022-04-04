namespace TeenControlSystemWeb.Exceptions.Sensor;

public class InvalidMacException : SystemException
{
    public InvalidMacException() : base("Некорректный MAC адрес")
    {
        
    }
}