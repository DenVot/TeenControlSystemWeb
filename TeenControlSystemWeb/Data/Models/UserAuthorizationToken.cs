using System;
using System.Collections.Generic;

namespace TeenControlSystemWeb.Data.Models
{
    public partial class UserAuthorizationToken
    {
        public long Id { get; set; }
        public string Token { get; set; } = null!;
        public long OwnerId { get; set; }

        public virtual User Owner { get; set; } = null!;
    }
}
