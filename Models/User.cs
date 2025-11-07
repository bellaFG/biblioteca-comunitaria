namespace BibliotecaComunitaria.Models
{
    public class User : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;
        public bool Ative { get; set; } = true;

        public ICollection<Book>? Books { get; set; }
        public ICollection<Borrow>? Borrow { get; set; }
    }
}
