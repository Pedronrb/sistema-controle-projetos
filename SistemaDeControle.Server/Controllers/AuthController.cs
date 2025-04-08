using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using sistemadecontrole.Server.Data;
using sistemadecontrole.Server.Helpers;
using sistemadecontrole.Server.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace sistemadecontrole.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public AuthController(AppDbContext context, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }
        /*{
            "email": "pedro@email.com",
            "senha": "123456"
        }*/
        //Bearer:   "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIzIiwidW5pcXVlX25hbWUiOiJQZWRybyBUZXN0ZSIsInJvbGUiOiJBbHVubyIsIm5iZiI6MTc0NDE0MDEwNSwiZXhwIjoxNzQ0MTgzMzA1LCJpYXQiOjE3NDQxNDAxMDUsImlzcyI6IlNpc3RlbWFEZUNvbnRyb2xlIiwiYXVkIjoiU2lzdGVtYURlQ29udHJvbGVVc3VhcmlvcyJ9.15UkUzWLtqMBKIDW7GVqqSZTmg0fg7MLjg77ohCqlvU"
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.Usuarios.SingleOrDefault(u =>
                u.Email == request.Email && u.Senha == request.Senha);

            if (user == null)
                return Unauthorized("Credenciais inválidas.");

            var token = GerarToken(user);
            return Ok(new { token });
        }
        /*
         {
            "nome": "Pedro Teste",
            "email": "pedro@email.com",
            "senha": "123456",
            "tipo": "Aluno"
          }
         */
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (_context.Usuarios.Any(u => u.Email == request.Email))
                return BadRequest("Email já está em uso.");

            var novoUsuario = new Usuario
            {
                Nome = request.Nome,
                Email = request.Email,
                Senha = request.Senha,
                Tipo = request.Tipo
            };

            _context.Usuarios.Add(novoUsuario);
            _context.SaveChanges();

            var token = GerarToken(novoUsuario);
            return Ok(new { token });
        }

        private string GerarToken(Usuario user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Nome),
                    new Claim(ClaimTypes.Role, user.Tipo),
                }),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpireHours),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Tipo { get; set; } = "Aluno"; // ou "Professor"
    }
}
