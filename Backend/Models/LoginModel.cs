using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Ingrese el Correo")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Ingrese la Contraseña")]
        public string Password { get; set; }
    }
}
