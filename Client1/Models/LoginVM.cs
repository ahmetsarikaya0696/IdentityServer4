using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Client1.Models
{
    public class LoginVM
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, PasswordPropertyText]
        public string Password { get; set; }
    }
}
