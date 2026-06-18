namespace MiWebApi.Models
{
    public class Obra
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? Estado { get; set; }
        public decimal Presupuesto { get; set; }
        public int IdCliente { get; set; }
    }
}
