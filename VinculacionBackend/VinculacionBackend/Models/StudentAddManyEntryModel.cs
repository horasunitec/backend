using System.ComponentModel.DataAnnotations;
using VinculacionBackend.CustomDataNotations;

namespace VinculacionBackend.Models
{
    public class StudentAddManyEntryModel
    {
        [Required(ErrorMessage = "*Numero de cuenta requerido")]
        [AccountNumberExist(ErrorMessage = "El numero de cuenta ya existe")]
        public string AccountId { get; set; }
        [Required(ErrorMessage = "*Nombre requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*Carrera requerida")]
        public string Major { get; set; }
        [Required(ErrorMessage = "*Correo electronico requerido")]
        [EmailExist(ErrorMessage = "El correo ya existe en la base de datos")]
        [ValidDomain(ErrorMessage = "Correo no valido")]
        public string Email { get; set; }
    }
}