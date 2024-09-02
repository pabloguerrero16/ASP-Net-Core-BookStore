using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {

        private readonly LibreriaDbContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public LibroController(LibreriaDbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet("Autores")]
        public async Task<IActionResult> GetAutores()
        {
            var autores = await _context.Autors.ToListAsync();
            return Ok(autores);
        }

        [HttpGet("Generos")]
        public async Task<IActionResult> GetGeneros()
        {
            var generos = await _context.Generos.ToListAsync();
            return Ok(generos);
        }

        [HttpGet("Formatos")]
        public async Task<IActionResult> GetFormatos()
        {
            var formatos = await _context.Formatos.ToListAsync();
            return Ok(formatos);
        }

        [HttpGet("GetLibros")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibros()
        {
            var libros = await _context.Libros
                .Include(l => l.Autor)
                .Include(l => l.Genero)
                .Include(l => l.Formato)
                .ToListAsync();

            return Ok(libros);
        }

        [HttpPost("AgregarLibro")]
        public async Task<IActionResult> AgregarLibro([FromForm] Libro libro)
        {
            if (ModelState.IsValid)
            {
                string imageUrl = null;
                if(libro.Imagen != null)
                {
                    imageUrl = await _cloudinaryService.UploadImageAsync(libro.Imagen);
                }

                var nuevoLibro = new Libro
                {
                    Nombre = libro.Nombre,
                    Stock = libro.Stock,
                    AutorId = libro.AutorId,
                    GeneroId = libro.GeneroId,
                    FormatoId = libro.FormatoId,
                    Precio = libro.Precio,
                    Descripcion = libro.Descripcion,
                    RutaFoto = imageUrl
                };

                _context.Libros.Add(nuevoLibro);
                await _context.SaveChangesAsync();
                return Ok(libro);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("EditarLibro/{id}")]
        public async Task<IActionResult> EditarLibro(int id, [FromForm] Libro libro)
        {
            if (ModelState.IsValid)
            {
                var libroExistente = await _context.Libros.FindAsync(id);
                if (libroExistente == null)
                {
                    return NotFound(new { message = "El libro no fue encontrado." });
                }

                if (libro.Imagen != null)
                {
                    libroExistente.RutaFoto = await _cloudinaryService.UploadImageAsync(libro.Imagen);
                }

                libroExistente.Nombre = libro.Nombre;
                libroExistente.Stock = libro.Stock;
                libroExistente.AutorId = libro.AutorId;
                libroExistente.GeneroId = libro.GeneroId;
                libroExistente.FormatoId = libro.FormatoId;
                libroExistente.Precio = libro.Precio;
                libroExistente.Descripcion = libro.Descripcion;

                _context.Libros.Update(libroExistente);
                await _context.SaveChangesAsync();

                return Ok(libroExistente);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("ObtenerLibro/{id}")]
        public async Task<IActionResult> ObtenerLibro(int id)
        {
            var libro = await _context.Libros
                                      .Include(l => l.Autor)
                                      .Include(l => l.Genero)
                                      .Include(l => l.Formato)
                                      .FirstOrDefaultAsync(l => l.Id == id);

            if (libro == null)
            {
                return NotFound(new { message = "El libro no fue encontrado." });
            }

            return Ok(libro);
        }

        [HttpDelete("EliminarLibro/{id}")]
        public async Task<IActionResult> EliminarLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);

            if (libro == null)
            {
                return NotFound(new { message = "El libro no fue encontrado." });
            }

            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();

            return Ok(new { message = "El libro ha sido eliminado exitosamente." });
        }
    }
}
