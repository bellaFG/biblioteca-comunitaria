using System.ComponentModel.DataAnnotations;

namespace BibliotecaComunitaria.Dto
{

    public class CreateUserDto
    {
        [Required, MaxLength(200)]
        public string Name { get; set; }

        [Required, MaxLength(200), EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

    }
}