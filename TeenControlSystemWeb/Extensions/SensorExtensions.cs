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

    public static SensorType ConvertToApiType(this Sensor sensor, bool includeActiveSession = true)
    {
        return new SensorType()
        {
            Id = sensor.Id,
            Name = sensor.Name,
            Mac = sensor.Mac,
            Online = sensor.Online,
            ActiveSession = includeActiveSession ? sensor.Session?.ConvertToApiType(false) ?? null : null 
        };
    }
}