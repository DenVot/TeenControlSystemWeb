namespace TeenControlSystemWeb.Types;

public class SessionType
{
    public long Id { get; set; }
    public string Name { get; set; }
    public UserType? Owner { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public IEnumerable<SensorType> Sensors { get; set; }
    public IEnumerable<PointType> Points { get; set; }
}