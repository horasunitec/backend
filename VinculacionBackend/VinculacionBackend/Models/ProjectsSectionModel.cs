using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class ProjectsSectionModel
    {
        [Required(ErrorMessage = "*Lista de proyectos requerida")]
        public List<long> ProjectIds { get; set; }
        [Required(ErrorMessage = "*Seccion requerida")]
        public long SectionId { get; set; }
    }
}