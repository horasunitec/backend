using System.ComponentModel.DataAnnotations;
using VinculacionBackend.CustomDataNotations;

namespace VinculacionBackend.Models
{
    public class EnableProfessorModel
    {
        [Required(ErrorMessage = "*Numero de cuenta requerido")]
        public string AccountId { get; set; }
        [Required(ErrorMessage = "*Correo electronico requerido")]
        [ValidDomain(ErrorMessage = "Correo electronico debe ser de dominio @unitec.edu o @unitec.edu.hn.")]
        [EmailExist(ErrorMessage = "Correo electronico ya existe en la Base de Datos.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "*Contraseña requerida")]
        public string Password { get; set; }
        [Required(ErrorMessage = "*Nombre requerido")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "*Apellido requerido")]
        public string LastName { get; set; }
    }
}