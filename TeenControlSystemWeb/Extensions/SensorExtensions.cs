using TeenControlSystemWeb.Exceptions.Sensor;

namespace TeenControlSystemWeb.Extensions;

public static class SensorExtensions
{
    public static void BindSensorToSession(this Sensor sensor, Session session)
    {
        if (sensor.SessionId != null)
        {
            throw new SensorAlreadyInUseException();
        }
            
        sensor.Session = session;
    }
}