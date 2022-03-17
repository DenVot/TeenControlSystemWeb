namespace TeenControlSystemWeb.Exceptions.Sensor;

public class SensorNotFoundException : SystemException
{
    public SensorNotFoundException() : base("Маячок не найден")
    {
        
    }
}