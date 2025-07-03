using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Client.Blazor.Models.Auth
{
    public class LoginModel
    {
        [Required(ErrorMessage = "نام کاربری الزامی است")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "رمز عبور الزامی است")]
        public string? Password { get; set; }
    }
}
