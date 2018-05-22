using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class VerifiedModel
    {
        [Required(ErrorMessage = "*Numero de Cuenta requerido")]
        public string AccountId { get; set; }
    }
}