using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class SectionEntryModel
    {
        [Required(ErrorMessage = "*Codigo requerido")]
        public string Code { get; set; }
        [Required(ErrorMessage = "*Clase requerido")]
        public long ClassId { get; set; }
        [Required(ErrorMessage = "*Docente requerido")]
        public string ProffesorAccountId { get; set; }
    }
}