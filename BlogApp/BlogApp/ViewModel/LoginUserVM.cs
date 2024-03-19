using Microsoft.Build.Framework;

namespace BlogApp.ViewModel
{
    public class LoginUserVM
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
