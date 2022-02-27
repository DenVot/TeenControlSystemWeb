namespace TeenControlSystemWeb.Exceptions.Sensor;

public class SensorAlreadyInUseException : SystemException
{
    public SensorAlreadyInUseException(long id) : base("Sensor with following id is already in use: " + id)
    {
        
    }
}