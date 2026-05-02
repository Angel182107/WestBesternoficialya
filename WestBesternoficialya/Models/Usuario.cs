namespace WestBesternoficialya.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Encriptada, por supuesto
        public int DepartamentoId { get; set; }
        public Departamento Departamento { get; set; }
    }
}
