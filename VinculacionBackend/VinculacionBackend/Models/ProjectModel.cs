using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VinculacionBackend.CustomDataNotations;

namespace VinculacionBackend.Models
{
    public class ProjectModel
    {
        [Required(ErrorMessage = "*Nombre requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*Descripcion requerido")]
        public string Description { get; set; }
        [Required(ErrorMessage = "*Carrera requerida")]
        [MajorListIsNotEmpty(ErrorMessage = "*lista no puede ir vacia")]
        public List<string> MajorIds { get; set; }
    }
}
