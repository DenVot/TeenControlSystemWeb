namespace TeenControlSystemWeb.Extensions;

public static class UserExtensions
{
    public static UserType ConvertToApiType(this User user) => new()
    {
        Id = user.Id,
        Username = user.Username,
        ActiveSession = user.Session?.ConvertToApiType(),
        IsAdmin = user.IsAdmin,
        AvatarId = user.DefaultAvatarId
    };
}