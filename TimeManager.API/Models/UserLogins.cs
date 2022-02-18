using System.ComponentModel.DataAnnotations;

namespace TimeManager.API.Models
{
    public class UserLogins
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public UserLogins() { }
    }
}
