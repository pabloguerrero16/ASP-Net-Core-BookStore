using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Frontend.Models;

namespace Frontend.Models
{
    public class Detalle
    {
        [Key]
        public int DetalleId { get; set; }

        public int MaestroId { get; set; }

        public int ProductoId { get; set; }

        public int CantidadCompra { get; set; }

        public decimal PrecioCompra { get; set; }

        public decimal Impuesto { get; set; }
    }
}
