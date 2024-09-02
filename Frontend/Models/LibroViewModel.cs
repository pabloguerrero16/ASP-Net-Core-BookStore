namespace Frontend.Models
{
    public class LibroViewModel
    {
        public IEnumerable<Libro> Libros { get; set; }
        public IEnumerable<Formato> Formatos { get; set; }
        public IEnumerable<Genero> Generos { get; set; }
        public IEnumerable<Autor> Autores { get; set; }

        public string NoResultsMessage { get; set; }
    }
}
