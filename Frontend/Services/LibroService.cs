using Frontend.Models;
using Newtonsoft.Json;

namespace Frontend.Services
{
    public class LibroService : ILibroService
    {
        private readonly HttpClient _httpClient;

        public LibroService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Autor>> GetAutoresAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:7281/api/Libro/Autores");
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Autor>>(json);
        }

        public async Task<List<Genero>> GetGenerosAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:7281/api/Libro/Generos");
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Genero>>(json);
        }

        public async Task<List<Formato>> GetFormatosAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:7281/api/Libro/Formatos");
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Formato>>(json);
        }
    }
}
