
namespace BibliotecaComunitaria.Models
{
    public class Book : Entity
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public BookStatus Status { get; set; } = BookStatus.Available;

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public ICollection<Borrow>? Borrow { get; set; }
    }

    public enum BookStatus
    {
        Available,
        CheckedOut
    }
}
