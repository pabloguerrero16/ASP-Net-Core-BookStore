using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Frontend.Controllers
{
    public class AdminController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly ILibroService _libroService;
        public AdminController(HttpClient httpClient, ILibroService libroService)
        {
            _httpClient = httpClient;
            _libroService = libroService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AgregarLibro()
        {
            var autores = await _libroService.GetAutoresAsync();
            var generos = await _libroService.GetGenerosAsync();
            var formatos = await _libroService.GetFormatosAsync();
            ViewBag.Autores = autores;
            ViewBag.Generos = generos;
            ViewBag.Formatos = formatos;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AgregarLibro(Libro ent)
        {
            if (ModelState.IsValid)
            {
                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(ent.Nombre), "Nombre");
                formData.Add(new StringContent(ent.Stock.ToString()), "Stock");
                formData.Add(new StringContent(ent.AutorId.ToString()), "AutorId");
                formData.Add(new StringContent(ent.GeneroId.ToString()), "GeneroId");
                formData.Add(new StringContent(ent.FormatoId.ToString()), "FormatoId");
                formData.Add(new StringContent(ent.Precio.ToString()), "Precio");
                formData.Add(new StringContent(ent.Descripcion), "Descripcion");

                if(ent.Imagen != null)
                {
                    var fileContent = new StreamContent(ent.Imagen.OpenReadStream());
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "Imagen",
                        FileName = ent.Imagen.FileName
                    };
                    formData.Add(fileContent);
                }

                var response = await _httpClient.PostAsync("https://localhost:7281/api/Libro/AgregarLibro", formData);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ListarLibros");
                }

                ModelState.AddModelError(string.Empty, "Error al agregar el libro");
            }

            return View(ent);
            
        }

        [HttpGet]
        public async Task<IActionResult> ListarLibros()
        {
            var response = await _httpClient.GetAsync("https://localhost:7281/api/Libro/GetLibros");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var libros = JsonConvert.DeserializeObject<List<Libro>>(responseData);
                return View(libros);
            }

            return View(new List<Libro>());
        }

        [HttpGet]
        public async Task<IActionResult> EditarLibro(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7281/api/Libro/ObtenerLibro/{id}");

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var libro = JsonConvert.DeserializeObject<Libro>(responseData);

                if (libro == null)
                {
                    return NotFound();
                }

                var autores = await _libroService.GetAutoresAsync();
                var generos = await _libroService.GetGenerosAsync();
                var formatos = await _libroService.GetFormatosAsync();

                ViewBag.Autores = autores;
                ViewBag.Generos = generos;
                ViewBag.Formatos = formatos;

                return View(libro);
            }

            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> EditarLibro(Libro ent)
        {
            if (ModelState.IsValid)
            {
                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(ent.Nombre), "Nombre");
                formData.Add(new StringContent(ent.Stock.ToString()), "Stock");
                formData.Add(new StringContent(ent.AutorId.ToString()), "AutorId");
                formData.Add(new StringContent(ent.GeneroId.ToString()), "GeneroId");
                formData.Add(new StringContent(ent.FormatoId.ToString()), "FormatoId");
                formData.Add(new StringContent(ent.Precio.ToString()), "Precio");
                formData.Add(new StringContent(ent.Descripcion), "Descripcion");

                if (ent.Imagen != null)
                {
                    var fileContent = new StreamContent(ent.Imagen.OpenReadStream());
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "Imagen",
                        FileName = ent.Imagen.FileName
                    };
                    formData.Add(fileContent);
                }

                var response = await _httpClient.PutAsync($"https://localhost:7281/api/Libro/EditarLibro/{ent.Id}", formData);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ListarLibros");
                }

                ModelState.AddModelError(string.Empty, "Error al editar el libro");
            }

            var autores = await _libroService.GetAutoresAsync();
            var generos = await _libroService.GetGenerosAsync();
            var formatos = await _libroService.GetFormatosAsync();

            // Reestablecer los ViewBags
            ViewBag.Autores = autores;
            ViewBag.Generos = generos;
            ViewBag.Formatos = formatos;

            return View(ent);
        }

        [HttpPost]
        [ActionName("EliminarLibro")]
        public async Task<IActionResult> EliminarLibroPost(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7281/api/Libro/EliminarLibro/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListarLibros");
            }

            ModelState.AddModelError(string.Empty, "Error al eliminar el libro");
            return View("ListarLibros");
        }

        // AUTORES

        [HttpPost]
        public async Task<IActionResult> AgregarAutor(Autor autor)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = "https://localhost:7281/api/Autors";
                var content = new StringContent(JsonConvert.SerializeObject(autor), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ListarAutores");
                }
                else
                {
                    ModelState.AddModelError("", "No se pudo agregar el autor. Intente nuevamente.");
                }
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]        
        public async Task<IActionResult> ListarAutores()
        {
            var response = await _httpClient.GetAsync("https://localhost:7281/api/Autors");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var autores = JsonConvert.DeserializeObject<List<Autor>>(responseData);
                return View(autores);
            }

            return View(new List<Autor>());
        }

        [HttpPost]
        [ActionName("EliminarAutor")]
        public async Task<IActionResult> EliminarAutorPost(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7281/api/Autors/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListarAutores");
            }

            ModelState.AddModelError(string.Empty, "Error al eliminar el autor");
            return View("ListarAutores");
        }

        // Generos

        [HttpPost]
        public async Task<IActionResult> AgregarGenero(Autor autor)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = "https://localhost:7281/api/Generoes";
                var content = new StringContent(JsonConvert.SerializeObject(autor), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ListarGeneros");
                }
                else
                {
                    ModelState.AddModelError("", "No se pudo agregar el Genero. Intente nuevamente.");
                }
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        public async Task<IActionResult> ListarGeneros()
        {
            var response = await _httpClient.GetAsync("https://localhost:7281/api/Generoes");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var generos = JsonConvert.DeserializeObject<List<Genero>>(responseData);
                return View(generos);
            }

            return View(new List<Genero>());
        }

        [HttpPost]
        [ActionName("EliminarGenero")]
        public async Task<IActionResult> EliminarGeneroPost(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7281/api/Generoes/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListarGeneros");
            }

            ModelState.AddModelError(string.Empty, "Error al eliminar el autor");
            return View("ListarGeneros");
        }


    }


}
