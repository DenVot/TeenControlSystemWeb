using System;
using System.Collections.Generic;

namespace TeenControlSystemWeb.Data.Models
{
    public partial class DefaultAvatar
    {
        public DefaultAvatar()
        {
            Users = new HashSet<User>();
        }

        public long Id { get; set; }
        public byte[] Avatar { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
