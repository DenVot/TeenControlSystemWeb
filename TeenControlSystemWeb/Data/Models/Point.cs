using System;
using System.Collections.Generic;

namespace TeenControlSystemWeb.Data.Models
{
    public partial class Point
    {
        public Point()
        {
            SessionFroms = new HashSet<Session>();
            SessionTos = new HashSet<Session>();
        }

        public long Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime AddedAt { get; set; }
        public long SessionId { get; set; }

        public virtual Session Session { get; set; } = null!;
        public virtual ICollection<Session> SessionFroms { get; set; }
        public virtual ICollection<Session> SessionTos { get; set; }
    }
}
