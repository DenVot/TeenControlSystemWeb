namespace TeenControlSystemWeb.Types;

public class RegisterSessionType
{
    public string Name { get; set; }
    public long OwnerId { get; set; }
    public DateTime StartAt { get; set; }
    public long[] SensorsIds { get; set; }
}