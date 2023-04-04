using System.ComponentModel.DataAnnotations;

namespace backend.model
{
    public partial class Login
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}