using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class LoginUserModel
    {
        [Required(ErrorMessage = "*Usuario requerido")]
        public string User { get; set; }
        [Required(ErrorMessage = "*Contrasena requerido")]
        public string Password { get;  set; }
    }
}