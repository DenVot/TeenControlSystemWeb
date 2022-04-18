namespace TeenControlSystemWeb.Exceptions.Sensor;

public class InvalidOrderException : SystemException
{
    public InvalidOrderException() : base("Такой порядковый номер уже существует")
    {
        
    }
}