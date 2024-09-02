using System;
using System.Collections.Generic;

namespace Frontend.Models;

public partial class Tarjetum
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public string Numero { get; set; } = null!;

    public string NombreTitular { get; set; } = null!;

    public DateOnly FechaExpiracion { get; set; }

    public string Cvv { get; set; } = null!;

    public virtual Usuario? Usuario { get; set; }
}
