using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public partial class Genero
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del género es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre del género no puede tener más de 100 caracteres.")]
    public string Nombre { get; set; } = null!;

    [NotMapped]
    public int CantidadLibros { get; set; }

    public virtual ICollection<Libro> Libros { get; set; } = new List<Libro>();
}
