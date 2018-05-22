using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class ProjectSectionModel
    {
        [Required(ErrorMessage = "*Proyecto requerido")]
        public long ProjectId { get; set; }
        [Required(ErrorMessage = "*Seccion requerida")]
        public long SectionId { get; set; }
    }
}