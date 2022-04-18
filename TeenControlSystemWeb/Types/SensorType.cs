namespace TeenControlSystemWeb.Types;

public class SensorType
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Mac { get; set; }
    public SessionType? ActiveSession { get; set; }
    public bool? Online { get; set; }
    public int Order { get; set; }
}