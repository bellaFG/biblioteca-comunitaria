using BibliotecaComunitaria.Dto;
using BibliotecaComunitaria.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaComunitaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;

        public UserController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            try
            {
                var user = await _userService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    message = "Usuário criado com sucesso!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "Usuário não encontrado." });

            return Ok(user);
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            try
            {
                var user = await _userService.UpdateAsync(id, dto);
                return Ok(new { message = "Dados atualizados com sucesso.", user });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _userService.AuthenticateAsync(request.Email, request.Password, _config);
            if (result == null)
                return Unauthorized(new { message = "E-mail ou senha incorretos." });

            return Ok(new { message = "Login realizado com sucesso!", result });
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var result = await _userService.DeactivateAsync(id);
            if (!result)
                return NotFound(new { message = "Usuário não encontrado ou já inativo." });

            return Ok(new { message = "Usuário desativado com sucesso." });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
