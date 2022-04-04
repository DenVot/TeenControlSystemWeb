namespace TeenControlSystemWeb.Extensions;

public static class SessionExtensions
{
    public static SessionType ConvertToApiType(this Session session,
        bool includeSensors = true,
        bool includePoints = true,
        bool includeOwner = true) => new()
    {
        Name = session.Name,
        Id = session.Id,
        StartAt = session.StartAt,
        StartedAt = session.StartedAt,
        EndedAt = session.EndedAt,
        Sensors = includeSensors ? session.Sensors.ToList().Select(x => x.ConvertToApiType(false)) : null,
        Points = includePoints ? session.Points.ToList().Select(x => new PointType()
        {
            Latitude = x.Latitude,
            Longitude = x.Longitude
        }) : null,
        Owner = includeOwner ? session.Owner.ConvertToApiType() : null
    };
}