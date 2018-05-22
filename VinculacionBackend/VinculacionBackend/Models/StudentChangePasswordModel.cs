using System.ComponentModel.DataAnnotations;
using VinculacionBackend.CustomDataNotations;

namespace VinculacionBackend.Models
{
    public class 
        
        StudentChangePasswordModel
    {
        [Required(ErrorMessage = "*Numero de cuenta requerido")]
        [StudentIsInactive(ErrorMessage = "El estudiante ya esta activo o no existe")]
        [AccountNumberDosentExist(ErrorMessage = "El numero de cuenta no existe")]
        public string AccountId { get; set; }
        [Required(ErrorMessage = "*Correo electronico requerido")]
        [EmailDosentExist(ErrorMessage = "El correo no existe en la base de datos")]
        [ValidDomain(ErrorMessage = "Correo no valido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "*Contraseña requerida")]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


    }
}