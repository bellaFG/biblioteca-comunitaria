
using BibliotecaComunitaria.Dto;
using BibliotecaComunitaria.Models;

namespace BibliotecaComunitaria.Services
{
    public interface IUserService
    {
        Task<User> CreateAsync(CreateUserDto dto);
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> UpdateAsync(Guid id, UpdateUserDto dto);
        Task<object?> AuthenticateAsync(string email, string password, IConfiguration config);
        Task<bool> DeactivateAsync(Guid id);
        string GenerateJwtToken(User user, IConfiguration config);
    }
}
