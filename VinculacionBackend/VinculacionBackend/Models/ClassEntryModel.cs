using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class ClassEntryModel
    {
        [Required(ErrorMessage = "*Nombre requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*Codigo requerido")]
        public string Code { get; set; }
    }
}