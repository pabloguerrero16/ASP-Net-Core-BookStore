using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.CodeAnalysis.Scripting;
using BCrypt.Net;

namespace Frontend.Controllers
{
    public class UsuarioController : Controller
    {


        private readonly HttpClient _httpClient;
        public UsuarioController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7281/api");
        }

        [HttpGet]
        public IActionResult registrarUsuario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> registrarUsuario(Usuario ent)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(ent);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/Usuarios", content);
                if(response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    ModelState.AddModelError("Correo", "El correo electrónico ya está registrado.");
                }
                else if (!response.IsSuccessStatusCode)
                {

                    ModelState.AddModelError(string.Empty, "Error al registrar el Usuario");
                }

                if (!ModelState.IsValid)
                {
                    return View(ent);
                }

                return RedirectToAction("Index", "Home");
            }

            return View(ent);
        }

        [HttpGet]
        public IActionResult login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> login(LoginModel ent)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(ent);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/Usuarios/login", content);

                if (response.IsSuccessStatusCode)
                {

                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<LoginResponse>(responseContent);

                    if(result?.User != null)
                    {
                        var userJson = JsonConvert.SerializeObject(result.User);
                        var userBytes = Encoding.UTF8.GetBytes(userJson);
                        HttpContext.Session.Set("User", userBytes);
                        return RedirectToAction("Index", "Home");
                    } else
                    {
                        ModelState.AddModelError(string.Empty, "Error en la respuesta del servidor.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
                }
            }
            return View(ent);
        }

        public IActionResult logout()
        {
            HttpContext.Session.Remove("User");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> MisCompras()
        {
            var userBytes = HttpContext.Session.Get("User");
            Usuario user = null;
            if (userBytes != null)
            {
                var userJson = Encoding.UTF8.GetString(userBytes);
                user = JsonConvert.DeserializeObject<Usuario>(userJson);
            }
            if (user == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            long usuarioId = user.Id;

            var response = await _httpClient.GetAsync($"https://localhost:7281/api/Carrito/Facturas?q={usuarioId}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var facturas = JsonConvert.DeserializeObject<List<Maestro>>(responseContent);

                return View(facturas);
            }
            else
            {
                ModelState.AddModelError("", "No se pudo obtener la lista de compras.");
                return View(new List<Maestro>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> DetalleFactura(long maestroId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7281/api/Carrito/Detalle?q={maestroId}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var detalle = JsonConvert.DeserializeObject<List<DetalleViewModel>>(responseContent);
                return View(detalle);
            }
            else
            {
                // Maneja el error aquí
                ModelState.AddModelError("", "No se pudo obtener los detalles de la factura.");
                return View(new List<DetalleViewModel>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditarPerfil()
        {
            var userBytes = HttpContext.Session.Get("User");
            Usuario user = null;
            if (userBytes != null)
            {
                var userJson = Encoding.UTF8.GetString(userBytes);
                user = JsonConvert.DeserializeObject<Usuario>(userJson);
            }
            if (user == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var response = await _httpClient.GetAsync($"https://localhost:7281/api/Usuarios/{user.Id}");

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var usuario = JsonConvert.DeserializeObject<Usuario>(responseData);

                if (usuario == null)
                {
                    return NotFound();
                }

                return View(usuario);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarPerfil(UsuarioUpdateModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View("EditarPerfil", modelo);
            }

            var userBytes = HttpContext.Session.Get("User");
            Usuario user = null;
            if (userBytes != null)
            {
                var userJson = Encoding.UTF8.GetString(userBytes);
                user = JsonConvert.DeserializeObject<Usuario>(userJson);
            }

            modelo.Id = user.Id;

            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7281/api/Usuarios/ActualizarPerfil", modelo);

            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.Set("User", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(modelo)));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "No se pudo actualizar el perfil.");
                return View("EditarPerfil", modelo);
            }
        }


        [HttpPost]
        public async Task<IActionResult> CambiarContrasena(CambiarContrasenaModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View("EditarPerfil");
            }

            var userBytes = HttpContext.Session.Get("User");
            Usuario user = null;
            if (userBytes != null)
            {
                var userJson = Encoding.UTF8.GetString(userBytes);
                user = JsonConvert.DeserializeObject<Usuario>(userJson);
            }

            modelo.Id = user.Id;

            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7281/api/Usuarios/CambiarContrasena", modelo);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "No se pudo cambiar la contraseña.");
                return View("EditarPerfil");
            }
        }


    }
}
