using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController : ControllerBase
    {
        private readonly LibreriaDbContext _context;

        public CarritoController(LibreriaDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("RegistrarCarrito")]
        public async Task<IActionResult> RegistrarCarrito([FromBody] Carrito carrito)
        {
            if (carrito == null)
            {
                return BadRequest("El carrito no puede ser nulo.");
            }

            try
            {
                var usuarioIdParam = new SqlParameter("@UsuarioId", carrito.UsuarioId);
                var productoIdParam = new SqlParameter("@ProductoId", carrito.ProductoId);
                var cantidadParam = new SqlParameter("@Cantidad", carrito.Cantidad);
                var fechaCarritoParam = new SqlParameter("@FechaCarrito", carrito.FechaCarrito);

                var result = await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_RegistrarCarrito @UsuarioId, @ProductoId, @Cantidad, @FechaCarrito",
                    usuarioIdParam, productoIdParam, cantidadParam, fechaCarritoParam);

                return Ok("Carrito registrado con éxito.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al registrar el carrito: {ex.Message}");
            }
        }


        [HttpGet("ConsultarCarrito")]
        public async Task<IActionResult> ConsultarCarrito(long q)
        {
            var carrito = await (from x in _context.Carritos
                    join y in _context.Libros on x.ProductoId equals y.Id
                    join a in _context.Autors on y.AutorId equals a.Id
                    join g in _context.Generos on y.GeneroId equals g.Id
                    join f in _context.Formatos on y.FormatoId equals f.Id
                    where x.UsuarioId == q
                    select new CarritoViewModel
                    {
                        ProductoId= x.ProductoId,
                        CarritoId=x.CarritoId,
                        UsuarioId = x.UsuarioId,
                        FechaCarrito = x.FechaCarrito,
                        ImageUrl = y.RutaFoto,
                        Cantidad = x.Cantidad,
                        Precio=y.Precio,
                        Nombre = y.Nombre,
                        Autor = a.Nombre,
                        Genero = g.Nombre,
                        Formato = f.Nombre,
                        SubTotal = y.Precio * x.Cantidad,
                        Impuesto = (y.Precio * x.Cantidad) * 0.13M,
                        Total = (y.Precio * x.Cantidad) + (y.Precio * x.Cantidad) * 0.13M
                    }).ToListAsync();

            return Ok(carrito);
        }

        [HttpPost("PagarCarrito")]
        public async Task<IActionResult> PagarCarrito(Carrito carrito)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC PagarCarrito_SP @UsuarioId",
                    new SqlParameter("@UsuarioId", carrito.UsuarioId));

                return Ok("Carrito pagado con éxito.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al ejecutar PagarCarrito_SP: {ex.Message}");
            }
        }


        [HttpDelete]
        public async Task<IActionResult> EliminarProductoCarrito(long q)
        {
            try
            {
                var carritoIdParam = new SqlParameter("@CarritoId", q);

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_EliminarProductoCarrito @CarritoId",
                    carritoIdParam);

                var productoEliminado = await _context.Carritos
                    .AnyAsync(x => x.CarritoId == q);

                if (!productoEliminado)
                {
                    return Ok("Producto eliminado del carrito con éxito.");
                }
                else
                {
                    return NotFound("No se encontró el producto en el carrito.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al eliminar el producto del carrito: {ex.Message}");
            }
        }






        [HttpGet("Facturas")]
        public List<Maestro> Facturas(long q)
        {
            using(var context = _context)
            {
                return (from x in context.Maestros
                        where x.UsuarioId == q
                        select x).ToList();
            }
        }

        [HttpGet("Detalle")]
        public object Detalle(long q)
        {
            using(var context = _context)
            {
                return (from x in _context.Detalles
                        join y in _context.Libros on x.ProductoId equals y.Id
                        join a in _context.Autors on y.AutorId equals a.Id
                        join g in _context.Generos on y.GeneroId equals g.Id
                        join f in _context.Formatos on y.FormatoId equals f.Id
                        where x.MaestroId == q
                        select new
                        {
                            x.ProductoId,
                            x.PrecioCompra,
                            x.CantidadCompra,
                            x.Impuesto,
                            y.Nombre,
                            Autor = a.Nombre,
                            Genero = g.Nombre,
                            Formato = f.Nombre,
                            subTotal = x.PrecioCompra * x.CantidadCompra,
                            ImpuestoTotal = x.Impuesto * x.CantidadCompra,
                            Total = (x.PrecioCompra * x.CantidadCompra) + (x.Impuesto * x.CantidadCompra)
                        }).ToList();
            }
        }

    }
}
