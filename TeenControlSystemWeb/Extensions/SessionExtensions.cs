namespace TeenControlSystemWeb.Extensions;

public static class SessionExtensions
{
    public static SessionType ConvertToApiType(this Session session) => new SessionType()
    {
        Name = session.Name,
        Id = session.Id,
        StartAt = session.StartAt,
        StartedAt = session.StartedAt,
        EndedAt = session.EndedAt,
        Sensors = session.Sensors.AsEnumerable().Select(x => new SensorType()
        {
            Id = x.Id,
            ActiveSession = null,
            Mac = x.Mac,
            Name = x.Name,
            Online = x.Online ?? false
        })
    };
}