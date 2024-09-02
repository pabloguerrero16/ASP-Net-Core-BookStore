using Frontend.Models;

namespace Frontend.Services
{
    public interface ILibroService
    {
        Task<List<Autor>> GetAutoresAsync();
        Task<List<Genero>> GetGenerosAsync();
        Task<List<Formato>> GetFormatosAsync();

    }
}
