using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class Maestro
    {
        [Key]
        public int MaestroId { get; set; }

        public int UsuarioId { get; set; }

        public decimal TotalFactura { get; set; }

        public DateTime FechaCompra { get; set; }

        public virtual Usuario? Usuario { get; set; }
    }
}
