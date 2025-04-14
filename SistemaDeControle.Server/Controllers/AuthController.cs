using Microsoft.AspNetCore.Mvc;
using SistemaDeControle.Server.Data;
using SistemaDeControle.Server.Helpers;
using SistemaDeControle.Server.Models;
using SistemaDeControle.Server.Services;
using SistemaDeControle.Server.DTOs.Auth;

namespace SistemaDeControle.Server.Controllers
{
    [ApiController]
    [Route("api/v1/login")]
    [Tags("Login")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _jwtService.ValidateUser(request.Email, request.Senha);

            if (user == null)
                return Unauthorized("Credenciais inválidas.");

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }

    }
}
