using System.ComponentModel.DataAnnotations;
using VinculacionBackend.CustomDataNotations;

namespace VinculacionBackend.Models
{
    public class ProfessorEntryModel
    {
        [Required(ErrorMessage = "*Numero de cuenta requerido")]
        [AccountNumberExist(ErrorMessage = "El numero de cuenta ya existe")]
        public string AccountId { get; set; }
        [Required(ErrorMessage = "*Nombre requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*Contraseña requerida")]
        public string Password { get; set; }
        [Required(ErrorMessage = "*Campus requerido")]
        public string Campus { get; set; }
        [Required(ErrorMessage = "*Email requerido")]
        [EmailExist(ErrorMessage = "El correo electronico ya existe en la base de datos")]
        [ValidDomain(ErrorMessage = "Correo electronico no valido")]
        public string Email { get; set; }
    }
}