namespace TeenControlSystemWeb.Helpers;

public class SessionDelta
{
    public string? Name { get; set; } = null;
    public DateTime? StartAt { get; set; } = null;
    public long? OwnerId { get; set; } = null;
    public IEnumerable<long>? SensorsToRemove { get; set; } = null;
    public IEnumerable<long>? SensorsToAdd { get; set; } = null;
}