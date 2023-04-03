using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Project.Identity.Models
{
    public class UserCreateModel
    {
        [Required(ErrorMessage ="Kullanıcı Adı Gereklidir.")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Lütfen Bir Email Formatı Giriniz.")]
        [Required(ErrorMessage = "Email Gereklidir.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifre Gereklidir.")]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage ="Şifreler Aynı Değildir.")]
        [Required(ErrorMessage = "Şifre Tekrar Alanı Gereklidir.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "İsim Soyisim Gereklidir.")]
        public string FirstName { get; set; }
    }
}
