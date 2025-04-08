using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistemadecontrole.Server.Data;
using sistemadecontrole.Server.Models;
using sistemadecontrole.Server.Dtos;
using System.Security.Claims;
using System.Threading.Tasks;

namespace sistemadecontrole.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Professor")]
    public class ProjetosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjetosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProjetoRequest request)
        {
            // Recupera o ID do professor logado
            var coordenadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var professor = await _context.Usuarios.FindAsync(coordenadorId);
            if (professor == null || professor.Tipo.ToLower() != "professor")
            {
                return BadRequest("Somente professores podem criar projetos.");
            }

            var projeto = new Projeto
            {
                Nome = request.Nome,
                Descricao = request.Descricao,
                CoordenadorId = coordenadorId
            };

            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = projeto.Id }, projeto);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var coordenadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var projetos = await _context.Projetos
                .Where(p => p.CoordenadorId == coordenadorId)
                .Include(p => p.Coordenador)
                .Include(p => p.Vinculos)
                .ToListAsync();

            return Ok(projetos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var coordenadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var projeto = await _context.Projetos
                .Include(p => p.Coordenador)
                .Include(p => p.Vinculos)
                .FirstOrDefaultAsync(p => p.Id == id && p.CoordenadorId == coordenadorId);

            if (projeto == null)
                return NotFound();

            return Ok(projeto);
        }
    }
}
