using System.ComponentModel.DataAnnotations;

namespace Project.Identity.Models
{
    public class UserSignInModel
    {
        [Required(ErrorMessage ="Kullanıcı Adı Gereklidir.")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Parola Gereklidir.")]
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
