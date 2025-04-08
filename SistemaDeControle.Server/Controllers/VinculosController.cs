using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistemadecontrole.Server.Data;
using sistemadecontrole.Server.Models;

namespace sistemadecontrole.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VinculosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VinculosController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Vinculos
        [HttpPost]
        public async Task<ActionResult<VinculoProjeto>> PostVinculo(VinculoProjeto vinculo)
        {
            _context.Vinculos.Add(vinculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVinculo), new { id = vinculo.Id }, vinculo);
        }

        // GET: api/Vinculos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VinculoProjeto>>> GetVinculos()
        {
            return await _context.Vinculos
                .Include(v => v.Usuario)
                .Include(v => v.Projeto)
                .ToListAsync();
        }

        // GET: api/Vinculos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VinculoProjeto>> GetVinculo(int id)
        {
            var vinculo = await _context.Vinculos
                .Include(v => v.Usuario)
                .Include(v => v.Projeto)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vinculo == null)
                return NotFound();

            return vinculo;
        }
    }
}
