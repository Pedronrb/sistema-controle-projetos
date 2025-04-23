using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaDeControle.Server.Models;
using SistemaDeControle.Server.Services;
using SistemaDeControle.Server.DTOs;


namespace SistemaDeControle.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UsuarioCriacaoDTO dto)
        {
            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = dto.Senha,
                Tipo = dto.Tipo,
                AreaAtuacao = dto.Tipo == TipoUsuario.Professor ? dto.AreaAtuacao : null,
                Formacao = dto.Tipo == TipoUsuario.Professor ? dto.Formacao : null
            };

            var (isValid, errorMessage) = await _usuarioService.ValidarUsuarioAsync(usuario);
            if (!isValid)
                return BadRequest(errorMessage);

            var novoUsuario = await _usuarioService.CriarUsuarioAsync(usuario);
            return CreatedAtAction(nameof(GetById), new { id = novoUsuario.Id }, novoUsuario);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var usuarios = await _usuarioService.ListarUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _usuarioService.BuscarPorIdAsync(id);
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }
    }
}
