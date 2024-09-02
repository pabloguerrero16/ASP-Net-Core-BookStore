using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Grpc.Core;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly LibreriaDbContext _context;

        public UsuariosController(LibreriaDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool emailExiste = await _context.Usuarios
                .AnyAsync(u => u.Correo == usuario.Correo);

            if (emailExiste)
            {
                ModelState.AddModelError("Correo", "El correo electrónico ya está registrado.");
                return Conflict("El correo electrónico ya está registrado.");
            }

            // Encriptar Contraseña
            usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
            usuario.RolId = 2;
            try
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return Ok("OK");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error en el servidor" });
            }
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        // Login => api/Usuarios/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel login)
        {
            var user = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == login.Correo);

            if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            {
                return Unauthorized("Credenciales Incorrectas.");
            }

            return Ok(new {Message = "OK", User = user});
        }

        [HttpPut("ActualizarPerfil")]
        public async Task<IActionResult> ActualizarPerfil([FromBody] UsuarioUpdateModel modelo)
        {
            if (modelo == null)
            {
                return BadRequest("El modelo no puede ser nulo.");
            }

            var usuario = await _context.Usuarios.FindAsync(modelo.Id);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Actualizar solo los campos especificados
            usuario.Nombre = modelo.Nombre;
            usuario.Apellido = modelo.Apellido;
            usuario.Correo = modelo.Correo;
            usuario.Telefono = modelo.Telefono;

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(modelo.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPut("CambiarContrasena")]
        public async Task<IActionResult> CambiarContrasena([FromBody] CambiarContrasenaModel modelo)
        {
            if (modelo == null)
            {
                return BadRequest("El modelo no puede ser nulo.");
            }

            var usuario = await _context.Usuarios.FindAsync(modelo.Id);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Verificar la contraseña actual
            if (!BCrypt.Net.BCrypt.Verify(modelo.ContrasenaActual, usuario.Password))
            {
                return BadRequest("La contraseña actual es incorrecta.");
            }

            // Actualizar la contraseña
            usuario.Password = BCrypt.Net.BCrypt.HashPassword(modelo.ContrasenaNueva);

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(modelo.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

    }
}
