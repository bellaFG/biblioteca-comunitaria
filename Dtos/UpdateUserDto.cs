using System.ComponentModel.DataAnnotations;

namespace BibliotecaComunitaria.Dto
{

    public class UpdateUserDto
    {
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(6)]
        public string Password { get; set; }
    }
}