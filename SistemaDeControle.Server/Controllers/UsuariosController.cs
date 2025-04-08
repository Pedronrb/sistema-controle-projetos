using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistemadecontrole.Server.Data;
using sistemadecontrole.Server.Models;

namespace sistemadecontrole.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {
            if(usuario.Tipo == "Professor")
            {
                if (string.IsNullOrWhiteSpace(usuario.AreaAtuacao) || string.IsNullOrWhiteSpace(usuario.Formacao))
                {
                    return BadRequest("Atuação e formação tem que existir para o professor!!.");
                }
            }
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }
    }
}
