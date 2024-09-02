using System;
using System.Collections.Generic;

namespace Frontend.Models;

public partial class Formato
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Libro> Libros { get; set; } = new List<Libro>();
}
