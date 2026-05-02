namespace WestBesternoficialya.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int CantidadActual { get; set; }
        public int StockMinimo { get; set; }
        public int StockMaximo { get; set; }
        public int DepartamentoId { get; set; } // Quién es dueño de este stock (ej. Ama de llaves)
        public Departamento Departamento { get; set; }
    }
}
