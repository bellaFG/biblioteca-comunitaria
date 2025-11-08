using BibliotecaComunitaria.Models;

namespace BibliotecaComunitaria.Dtos
{
    public class UpdateBookDto
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int? PublicationYear { get; set; }
        public BookStatus? Status { get; set; }
    }
}
