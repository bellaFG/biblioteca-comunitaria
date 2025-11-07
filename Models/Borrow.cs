
namespace BibliotecaComunitaria.Models
{
    public class Borrow : Entity
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public DateTime CheckoutDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public User? User { get; set; }
        public Book? Book { get; set; }
    }
}
