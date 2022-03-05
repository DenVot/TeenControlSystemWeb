namespace TeenControlSystemWeb.Extensions;

public static class UserExtensions
{
    public static UserType ConvertToApiType(this User user) => new()
    {
        Id = user.Id,
        Username = user.Username,
        ActiveSession = user.Session != null
            ? new SessionType()
            {
                Name = user.Session.Name,
                Id = user.Session.Id,
                StartAt = user.Session.StartAt,
                StartedAt = user.Session.StartedAt,
                EndedAt = user.Session.EndedAt,
                Sensors = user.Session.Sensors.AsEnumerable().Select(x => new SensorType()
                {
                    Id = x.Id,
                    ActiveSession = null,
                    Mac = x.Mac,
                    Name = x.Name,
                    Online = x.Online ?? false
                })
            }
            : null,
        IsAdmin = user.IsAdmin
    };
}