using System;
using System.Collections.Generic;

namespace TeenControlSystemWeb.Data.Models
{
    public partial class User
    {
        public User()
        {
            Sessions = new HashSet<Session>();
            UserAuthorizationTokens = new HashSet<UserAuthorizationToken>();
        }

        public long Id { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordMd5Hash { get; set; } = null!;
        public bool IsAdmin { get; set; }
        public long? SessionId { get; set; }

        public virtual Session? Session { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
        public virtual ICollection<UserAuthorizationToken> UserAuthorizationTokens { get; set; }
    }
}
