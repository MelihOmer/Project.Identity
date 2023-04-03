using System.ComponentModel.DataAnnotations;

namespace Project.Identity.Models
{
    public class RoleCreateModel
    {
        [Required(ErrorMessage ="Rol Adı Gereklidir")]
        public string Name { get; set; }
    }
}
