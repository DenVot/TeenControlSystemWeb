namespace TeenControlSystemWeb.Exceptions.Sensor;

public class SensorNotFoundException : SystemException
{
    public SensorNotFoundException(long id) : base("No sensor was found with following id: " + id)
    {
        
    }
}