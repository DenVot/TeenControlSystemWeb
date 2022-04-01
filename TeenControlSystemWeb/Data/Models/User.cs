using System;
using System.Collections.Generic;

namespace TeenControlSystemWeb.Data.Models
{
    public partial class User
    {
        public User()
        {
            Sessions = new HashSet<Session>();
        }

        public long Id { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordMd5Hash { get; set; } = null!;
        public bool IsAdmin { get; set; }
        public long? SessionId { get; set; }
        public long DefaultAvatarId { get; set; }

        public virtual DefaultAvatar DefaultAvatar { get; set; } = null!;
        public virtual Session? Session { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
