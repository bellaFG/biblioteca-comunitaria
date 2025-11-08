using System.ComponentModel.DataAnnotations;

namespace BibliotecaComunitaria.Models
{
    public class Borrow : Entity
    {
        [Required]
        public Guid BookId { get; set; }
        public Book? Book { get; set; }

        [Required]
        public Guid BorrowerId { get; set; } // quem pegou o livro
        public User? Borrower { get; set; }

        [Required]
        public Guid OwnerId { get; set; } // dono do livro
        public User? Owner { get; set; }

        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnDate { get; set; }
        public bool Returned { get; set; } = false;
    }
}
