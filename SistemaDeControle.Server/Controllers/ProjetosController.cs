using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistemadecontrole.Server.Data;
using sistemadecontrole.Server.Models;

namespace sistemadecontrole.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjetosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjetosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Projeto projeto)
        {
            var coordenador = await _context.Usuarios.FindAsync(projeto.CoordenadorId);
            if (coordenador == null || coordenador.Tipo.ToLower() != "professor")
            {
                return BadRequest("Somente professores podem ser coordenadores de projeto.");
            }

            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = projeto.Id }, projeto);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var projetos = await _context.Projetos
                .Include(p => p.Coordenador)
                .Include(p => p.Vinculos)
                .ToListAsync();
            return Ok(projetos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var projeto = await _context.Projetos
                .Include(p => p.Coordenador)
                .Include(p => p.Vinculos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (projeto == null)
                return NotFound();

            return Ok(projeto);
        }
    }
}
