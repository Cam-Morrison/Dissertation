using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.model
{
    public partial class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; }
    }
}