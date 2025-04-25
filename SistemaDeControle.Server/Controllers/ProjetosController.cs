using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaDeControle.Server.Models;
using SistemaDeControle.Server.Data;
using SistemaDeControle.Server.DTOs.Projeto;
using System.Security.Claims;

namespace SistemaDeControle.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class ProjetosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjetosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CriarProjeto([FromBody] ProjetoCriacaoDTO projetoCriacaoDTO)
        {
            // Corre��o: Usando ClaimTypes.NameIdentifier para pegar o ID corretamente
            var usuarioIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdString) || !int.TryParse(usuarioIdString, out var usuarioId))
            {
                return Unauthorized("N�o foi poss�vel identificar o usu�rio.");
            }

            var usuario = await _context.Usuarios.FindAsync(usuarioId);

            if (usuario == null || usuario.Tipo != TipoUsuario.Professor)
            {
                return Unauthorized("Apenas professores podem criar projetos.");
            }

            // Cria��o do projeto
            var projeto = new Projeto
            {
                Nome = projetoCriacaoDTO.Nome,
                Descricao = projetoCriacaoDTO.Descricao,
            };

            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync();

            // Vincula o professor ao projeto como coordenador
            var vinculo = new VinculoProjeto
            {
                UsuarioId = usuario.Id,
                ProjetoId = projeto.Id,
                Funcao = Funcao.Master // O professor � o coordenador (Master)
            };

            _context.Vinculos.Add(vinculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CriarProjeto), new { id = projeto.Id }, projeto);
        }

        // Endpoint para listagem de projetos
        [HttpGet]
        public async Task<IActionResult> ListarProjetos()
        {
            var projetos = await _context.Projetos.ToListAsync();
            return Ok(projetos);
        }

        // Endpoint para detalhamento de projeto
        [HttpGet("{id}")]
        public async Task<IActionResult> DetalharProjeto(int id)
        {
            var projeto = await _context.Projetos
                .Include(p => p.Vinculos)
                .ThenInclude(v => v.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (projeto == null)
                return NotFound("Projeto n�o encontrado.");

            var projetoDTO = new ProjetoDTO
            {
                Id = projeto.Id,
                Nome = projeto.Nome,
                Descricao = projeto.Descricao,
                Vinculos = projeto.Vinculos.Select(v => new VinculoDTO
                {
                    AlunoNome = v.Usuario?.Nome ?? "Nome n�o dispon�vel",
                    Funcao = v.Funcao
                }).ToList()
            };

            return Ok(projetoDTO);
        }

        // Endpoint para edi��o de projeto
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarProjeto(int id, [FromBody] ProjetoDTO projetoDTO)
        {
            var projeto = await _context.Projetos.FindAsync(id);
            if (projeto == null)
                return NotFound("Projeto n�o encontrado.");

            projeto.Nome = projetoDTO.Nome;
            projeto.Descricao = projetoDTO.Descricao;

            _context.Projetos.Update(projeto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Endpoint para vincula��o de aluno a um projeto
        [HttpPost("{projetoId}/vincular-aluno")]
        public async Task<IActionResult> VincularAluno(int projetoId, [FromBody] VinculoAlunoDTO vinculoDTO)
        {
            var projeto = await _context.Projetos.FindAsync(projetoId);
            if (projeto == null)
                return NotFound("Projeto n�o encontrado.");

            var usuario = await _context.Usuarios.FindAsync(vinculoDTO.AlunoId);
            if (usuario == null || usuario.Tipo != TipoUsuario.Aluno)
                return BadRequest("Usu�rio n�o encontrado ou tipo de usu�rio inv�lido.");

            var vinculo = new VinculoProjeto
            {
                UsuarioId = usuario.Id,
                ProjetoId = projetoId,
                Funcao = vinculoDTO.Funcao
            };

            _context.Vinculos.Add(vinculo);
            await _context.SaveChangesAsync();

            return Ok(vinculo);
        }
    }
}
