using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class SectionProjectEntryModel
    {
        [Required(ErrorMessage = "*Seccion requerida")]
        public long SectiontId { get; set; }
        [Required(ErrorMessage = "*Lista de proyectos requerida")]
        public IList<long> ProjectIds { get; set; }
        [Required(ErrorMessage = "*Organizacion requerida")]
        public string Organization { get; set; }
        [Required(ErrorMessage = "*Descripcion requerida")]
        public string Description { get; set; }
        [Required(ErrorMessage = "*Costo requerido")]
        public double Cost { get; set; }
    }
}