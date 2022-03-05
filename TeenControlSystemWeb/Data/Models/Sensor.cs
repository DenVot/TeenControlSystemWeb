namespace TeenControlSystemWeb.Data.Models
{
    public partial class Sensor
    {
        public long Id { get; set; }
        public string Mac { get; set; } = null!;
        public string Name { get; set; } = null!;
        public long? SessionId { get; set; }
        public bool? Online { get; set; }

        public virtual Session? Session { get; set; }
    }
}
