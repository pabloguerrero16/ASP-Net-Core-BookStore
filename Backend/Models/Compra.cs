using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Compra
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public decimal Total { get; set; }

    public DateTime FechaCompra { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
