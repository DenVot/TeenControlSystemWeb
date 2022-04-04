namespace TeenControlSystemWeb.Exceptions.Sensor;

public class SensorAlreadyExistsException : SystemException 
{
    public SensorAlreadyExistsException() : base("Маячок уже существует")
    {
        
    }
}