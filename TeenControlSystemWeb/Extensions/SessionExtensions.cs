namespace TeenControlSystemWeb.Extensions;

public static class SessionExtensions
{
    public static SessionType ConvertToApiType(this Session session) => new()
    {
        Name = session.Name,
        Id = session.Id,
        StartAt = session.StartAt,
        StartedAt = session.StartedAt,
        EndedAt = session.EndedAt,
        Sensors = session.Sensors.ToList().Select(x => new SensorType()
        {
            Id = x.Id,
            ActiveSession = null,
            Mac = x.Mac,
            Name = x.Name,
            Online = x.Online ?? false
        }),
        Points = session.Points.ToList().Select(x => new PointType()
        {
            Latitude = x.Latitude,
            Longitude = x.Longitude
        }),
        Owner = new UserType()
        {
            Id = session.Owner.Id,
            AvatarId = session.Owner.DefaultAvatarId,
            IsAdmin = session.Owner.IsAdmin,
            Username = session.Owner.Username
        }
    };
}