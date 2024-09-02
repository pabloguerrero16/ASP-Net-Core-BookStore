using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http;
using System.Text;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly ILibroService _libroService;
        public HomeController(HttpClient httpClient, ILibroService libroService)
        {
            _httpClient = httpClient;
            _libroService = libroService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? formatoId, int? generoId, int? autorId, string searchQuery)
        {
            var response = await _httpClient.GetAsync("https://localhost:7281/api/Libro/GetLibros");
            var libros = response.IsSuccessStatusCode ?
                JsonConvert.DeserializeObject<List<Libro>>(await response.Content.ReadAsStringAsync()) :
                new List<Libro>();

            if (formatoId.HasValue)
                libros = libros.Where(libro => libro.FormatoId == formatoId.Value).ToList();

            if (generoId.HasValue)
                libros = libros.Where(libro => libro.GeneroId == generoId.Value).ToList();

            if (autorId.HasValue)
                libros = libros.Where(libro => libro.AutorId == autorId.Value).ToList();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                // Normalizar y eliminar acentos
                string NormalizeString(string str)
                {
                    var normalized = str.Normalize(NormalizationForm.FormD);
                    var stringBuilder = new StringBuilder();
                    foreach (var c in normalized)
                    {
                        var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                        if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                        {
                            stringBuilder.Append(c);
                        }
                    }
                    return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
                }

                var normalizedQuery = NormalizeString(searchQuery.ToLower());

                libros = libros
                    .Where(libro => NormalizeString(libro.Nombre.ToLower()).Contains(normalizedQuery))
                    .ToList();
            }

            var formatos = await _libroService.GetFormatosAsync();
            var generos = await _libroService.GetGenerosAsync();
            var autores = await _libroService.GetAutoresAsync();

            var viewModel = new LibroViewModel
            {
                Libros = libros,
                Formatos = formatos,
                Generos = generos,
                Autores = autores,
                NoResultsMessage = libros.Count == 0 ? "No se han encontrado resultados." : null
            };

            ViewData["CurrentSearch"] = searchQuery; // Para mantener el valor de búsqueda en la barra

            return View(viewModel);
        }



        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
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

                return View(libro);
            }

            return StatusCode((int)response.StatusCode);
        }

    }
}
