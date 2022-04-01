using System;
using System.Collections.Generic;

namespace TeenControlSystemWeb.Data.Models
{
    public partial class Point
    {
        public long Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime AddedAt { get; set; }
        public long SessionId { get; set; }

        public virtual Session Session { get; set; } = null!;
    }
}
