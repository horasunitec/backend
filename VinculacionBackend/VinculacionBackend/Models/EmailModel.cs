using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class EmailModel
    {
        [Required(ErrorMessage = "*Email requerido")]
        public string Email { get; set; }
    }
}