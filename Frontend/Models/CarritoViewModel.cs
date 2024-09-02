namespace Frontend.Models
{
    public partial class CarritoViewModel
    {
        public int ProductoId { get; set; }
        public int CarritoId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaCarrito { get; set; }
        public string ImageUrl { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public string Nombre { get; set; }
        public string Autor { get; set; }
        public string Genero { get; set; }
        public string Formato { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
    }

}
