using System.ComponentModel.DataAnnotations;

namespace WestBesternoficialya.Models.ViewModels
{
    public class PerfilViewModel
    {
        // Datos informativos (Solo lectura)
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Departamento { get; set; }

        // Campos para el cambio de contraseña
        [Required(ErrorMessage = "La contraseña actual es obligatoria")]
        [DataType(DataType.Password)]
        public string PasswordActual { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es obligatoria")]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string PasswordNueva { get; set; }

        [DataType(DataType.Password)]
        [Compare("PasswordNueva", ErrorMessage = "La nueva contraseña y la confirmación no coinciden.")]
        public string ConfirmarPassword { get; set; }
    }
}