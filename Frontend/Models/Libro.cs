using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Frontend.Models;

namespace Frontend.Models;

public partial class Libro
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del libro es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre del libro no puede tener más de 100 caracteres.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El autor es obligatorio.")]
    public int? AutorId { get; set; }

    [Required(ErrorMessage = "El género es obligatorio.")]
    public int? GeneroId { get; set; }

    [Required(ErrorMessage = "El formato es obligatorio.")]
    public int? FormatoId { get; set; }

    [StringLength(1000, ErrorMessage = "La descripción no puede tener más de 1000 caracteres.")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
    public decimal Precio { get; set; }

    [Required(ErrorMessage = "El stock es obligatorio.")]
    [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser un número positivo.")]
    public int Stock { get; set; }

    public string? RutaFoto { get; set; }

    [NotMapped]
    public IFormFile? Imagen { get; set; }

    public virtual Autor? Autor { get; set; }

    public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();

    public virtual Formato? Formato { get; set; }

    public virtual Genero? Genero { get; set; }
}
