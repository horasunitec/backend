using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class PeriodEntryModel
    {
        [Required(ErrorMessage = "*Numero de periodo requerido")]
        public int Number { get; set; }
        [Required(ErrorMessage = "*Año de periodo requerido")]
        public int Year { get; set; }
        [Required(ErrorMessage = "*Fecha Desde requerida")]
        public string FromDate { get; set; }
        [Required(ErrorMessage = "*Fecha Hasta requerida")]
        public string ToDate { get; set; }
    }
}