using System;
using System.Collections.Generic;

namespace backend.entity
{
    public partial class Task
    {
        public int TaskId { get; set; }
        public DateTime TaskTime { get; set; }
        public int ActionId { get; set; }
        public int UserId { get; set; }

        public virtual Action Action { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
