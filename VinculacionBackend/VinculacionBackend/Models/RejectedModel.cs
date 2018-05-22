using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class RejectedModel
    {
        [Required(ErrorMessage = "*Numero de cuenta requerido")]
        public string AccountId { get; set; }
        [Required(ErrorMessage = "*Mensaje requerido")]
        public string Message { get; set; }
    }
}