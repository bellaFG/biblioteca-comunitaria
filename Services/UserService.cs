
using BibliotecaComunitaria.Data;
using BibliotecaComunitaria.Dto;
using BibliotecaComunitaria.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BibliotecaComunitaria.Services
{
    public class UserService : IUserService
    {
        private readonly BibliotecaContext _context;

        public UserService(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<User> CreateAsync(CreateUserDto dto)
        {
            if (await _context.User.AnyAsync(u => u.Email == dto.Email.Trim().ToLower()))
                throw new InvalidOperationException("E-mail já cadastrado.");

            var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Name = dto.Name.Trim(),
                Email = dto.Email.Trim().ToLower(),
                Hash = hash,
                Ative = true
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.User
                .Where(u => u.Ative)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.User
                .Include(u => u.Books)
                .Include(u => u.Borrow)
                .FirstOrDefaultAsync(u => u.Id == id && u.Ative);
        }

        public async Task<User?> UpdateAsync(Guid id, UpdateUserDto dto)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == id && u.Ative);
            if (user == null)
                throw new KeyNotFoundException("Usuário não encontrado ou inativo.");

            if (!string.IsNullOrWhiteSpace(dto.Name))
                user.Name = dto.Name.Trim();

            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                var emailExists = await _context.User
                    .AnyAsync(u => u.Email == dto.Email.Trim().ToLower() && u.Id != id);
                if (emailExists)
                    throw new InvalidOperationException("E-mail já em uso por outro usuário.");
                user.Email = dto.Email.Trim().ToLower();
            }

            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.Hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<object?> AuthenticateAsync(string email, string password, IConfiguration config)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(u => u.Email == email.Trim().ToLower() && u.Ative);

            if (user == null) return null;

            bool valid = BCrypt.Net.BCrypt.Verify(password, user.Hash);
            if (!valid) return null;

            var token = GenerateJwtToken(user, config);

            return new
            {
                user.Id,
                user.Name,
                user.Email,
                Token = token
            };
        }

        public async Task<bool> DeactivateAsync(Guid id)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null || !user.Ative)
                return false;

            user.Ative = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public string GenerateJwtToken(User user, IConfiguration config)
        {
            var jwtSettings = config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("name", user.Name)
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
