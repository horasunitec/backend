using System.ComponentModel.DataAnnotations;
using VinculacionBackend.CustomDataNotations;

namespace VinculacionBackend.Models
{
    public class UserUpdateModel
    {
        [Required(ErrorMessage = "*Numero de cuenta requerido")]
        public string AccountId { get; set; }
        [Required(ErrorMessage = "*Nombre requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*Carrera requerida")]
        public string MajorId { get; set; }
        [Required(ErrorMessage = "*Campus requerido")]
        public string Campus { get; set; }
        [Required(ErrorMessage = "*Correo electronico requerido")]
        [ValidDomain(ErrorMessage = "Correo no valido")]
        public string Email { get; set; }
    }
}