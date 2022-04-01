using System;
using System.Collections.Generic;

namespace TeenControlSystemWeb.Data.Models
{
    public partial class Session
    {
        public Session()
        {
            Points = new HashSet<Point>();
            Sensors = new HashSet<Sensor>();
            Users = new HashSet<User>();
        }

        public long Id { get; set; }
        public long OwnerId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string Name { get; set; } = null!;

        public virtual User Owner { get; set; } = null!;
        public virtual ICollection<Point> Points { get; set; }
        public virtual ICollection<Sensor> Sensors { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
