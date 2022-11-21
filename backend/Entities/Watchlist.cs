using System;
using System.Collections.Generic;

namespace backend.entity
{
    public partial class Watchlist
    {
        public int WatchListId { get; set; }
        public int UserId { get; set; }
        public string WatchListName { get; set; } = null!;
        public string? Stocks { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
