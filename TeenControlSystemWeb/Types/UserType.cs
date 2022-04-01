namespace TeenControlSystemWeb.Types;

public class UserType
{
    public long Id { get; set; }
    public string Username { get; set; }
    public bool IsAdmin { get; set; }
    public SessionType? ActiveSession { get; set; }
    public long AvatarId { get; set; }
}