namespace TeenControlSystemWeb.Types;

public class SessionSnapshot
{
    public IDictionary<long, bool> Sensors { get; set; }
    public PointType Point { get; set; }
}