using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class SectionStudentModel
    {
        [Required(ErrorMessage = "*Seccion requerida")]
        public long SectionId { get; set; }
        [Required(ErrorMessage ="*Lista de estudiantes requerida")]
        public List<string> StudenstIds { get; set; }
    }
}