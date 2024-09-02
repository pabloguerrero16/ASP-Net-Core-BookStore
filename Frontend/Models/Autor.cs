using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frontend.Models;

public partial class Autor
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del autor es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre del autor no puede tener más de 100 caracteres.")]
    public string Nombre { get; set; } = null!;

    [NotMapped]
    public int CantidadLibros { get; set; }

    public virtual ICollection<Libro> Libros { get; set; } = new List<Libro>();
}
