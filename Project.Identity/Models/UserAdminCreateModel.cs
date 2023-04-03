using System.ComponentModel.DataAnnotations;

namespace Project.Identity.Models
{
    public class UserAdminCreateModel
    {
        [Required(ErrorMessage ="Kullanıcı Adı Gereklidir")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email Gereklidir")]
        public string Email { get; set; }
        [Required(ErrorMessage = "İsim Gereklidir")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Soyisim Gereklidir")]
        public string LastName { get; set; }
    }
}
