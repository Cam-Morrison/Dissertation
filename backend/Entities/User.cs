using System;
using System.Collections.Generic;

namespace backend.entity
{
    public partial class User
    {
        public User()
        {
            Actions = new HashSet<Action>();
            Tasks = new HashSet<Task>();
            Watchlists = new HashSet<Watchlist>();
        }

        public int? UserId { get; set; }
        public string UserName { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public bool? UserIsAdmin { get; set; } 
        public bool? UserIsAccountLocked { get; set; }
        public bool? UserAiSwitchedOnPreference { get; set; }

        public virtual ICollection<Action> Actions { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<Watchlist> Watchlists { get; set; }
    }
}
