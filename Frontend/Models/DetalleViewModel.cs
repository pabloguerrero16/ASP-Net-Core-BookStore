namespace Frontend.Models
{
    public class DetalleViewModel
    {
        public int ProductoId { get; set; }
        public decimal PrecioCompra { get; set; }
        public int CantidadCompra { get; set; }
        public decimal Impuesto { get; set; }
        public string Nombre { get; set; }
        public string Autor { get; set; }
        public string Genero { get; set; }
        public string Formato { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ImpuestoTotal { get; set; }
        public decimal Total { get; set; }
    }
}
