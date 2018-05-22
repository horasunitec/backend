using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class HourEntryModel
    {
        [Required(ErrorMessage = "*Numero de cuenta requerido")]
        public string AccountId { get; set; }
        [Required(ErrorMessage = "*Seccion requerida")]
        public long SectionId { get; set; }
        [Required(ErrorMessage = "*Proyecto requerido")]
        public long ProjectId{ get; set; }
        [Required(ErrorMessage = "*Cantidad de horas requerido")]
        public int Hour { get; set; }
    }
}