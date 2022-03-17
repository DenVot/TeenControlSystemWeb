namespace TeenControlSystemWeb.Exceptions.Sensor;

public class SensorAlreadyInUseException : SystemException
{
    public SensorAlreadyInUseException() : base("Маячок уже используется")
    {
        
    }
}