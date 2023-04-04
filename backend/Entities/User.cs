namespace backend.entity
{
    public partial class User
    {
        public User()
        {
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

        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<Watchlist> Watchlists { get; set; }
        
    }
}
