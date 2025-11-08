using System.ComponentModel.DataAnnotations;

namespace BibliotecaComunitaria.Models
{
    public class Book : Entity
    {
        [Required, MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Author { get; set; } = string.Empty;

        [Range(0, 2100)]
        public int PublicationYear { get; set; }

        public BookStatus Status { get; set; } = BookStatus.Available;

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public ICollection<Borrow>? Borrows { get; set; }
    }

    public enum BookStatus
    {
        Available,
        CheckedOut
    }
}

