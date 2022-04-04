namespace TeenControlSystemWeb.Extensions;

public static class UserExtensions
{
    public static UserType ConvertToApiType(this User user, bool includeSession = true) => new()
    {
        Id = user.Id,
        Username = user.Username,
        ActiveSession = includeSession ? user.Session?.ConvertToApiType(includeOwner: false) : null,
        IsAdmin = user.IsAdmin,
        AvatarId = user.DefaultAvatarId
    };
}