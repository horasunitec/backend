using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class VerifiedProfessorModel
    {
        [Required(ErrorMessage = "*Numero de Cuentabrequerido")]
        public string AccountId { get; set; }

        [Required(ErrorMessage = "*Contraseña requerida")]
        public string Password { get; set; }
    }
}