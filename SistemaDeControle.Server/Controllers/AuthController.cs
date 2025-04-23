using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using sistemadecontrole.Server.Services;

using System;
using SistemaDeControle.Server.DTOs;
using SistemaDeControle.Server.Models;

namespace SistemaDeControle.Server.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly JwtService _jwtService;
        private readonly IConfiguration _config;

        public AuthController(AuthService authService, JwtService jwtService, IConfiguration config)
        {
            _authService = authService;
            _jwtService = jwtService;
            _config = config;
        }

        [HttpPost("login/admin")]
        public IActionResult LoginAdmin([FromBody] AdminRequest request)
        {
            var adminPassword = _config["Admin:Password"];
            var adminCode = _config["Admin:Code"];

            if (request.Password != adminPassword || request.Code != adminCode)
                return Unauthorized("Acesso negado.");

            var token = _jwtService.GenerateToken("admin", "ADMIN", 0);
            return Ok(new { token });
        }

        [HttpPost("login/{tipo}")]
        public IActionResult LoginUsuario([FromRoute] TipoUsuario tipo, [FromBody] UsuarioRequest request)
        {
            if (!_authService.VerificarUsuario(request.Email, request.Senha, tipo.ToString()))
                return Unauthorized("Credenciais inválidas.");

            var token = _jwtService.GenerateToken(request.Email, tipo.ToString(), 0);
            return Ok(new { token });
        }
    }
}
