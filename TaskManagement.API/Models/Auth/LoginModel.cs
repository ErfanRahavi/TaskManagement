using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Models.Auth
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}
