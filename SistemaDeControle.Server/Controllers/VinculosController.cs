using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistemadecontrole.Server.Data;
using sistemadecontrole.Server.Models;
using sistemadecontrole.Server.DTOs;
/*
 [Post]
 {
  "usuarioId": 3,
  "projetoId": 2,
  "funcao": "Júnior"  Fucao: {estágio, júnior, sênior, master}
}
 */
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
        public async Task<ActionResult<VinculoProjeto>> PostVinculo(VinculoProjetoDTO vinculo)
        {
            //var vinculo e o DTO, n gera o ID com BD. Criar um obj completo com vinculo
            //_context e a instacia que faz a conexao com o BD
            var usuario = await _context.Usuarios.FindAsync(vinculo.UsuarioId);
            var projeto = await _context.Projetos.FindAsync(vinculo.ProjetoId);

            //garantir a existencia dos objs. Se n exitir pelo menos um, nao faz sentido a vinculacao
            if (usuario == null || projeto == null) { 
                return BadRequest("Usuario ou Projeto não existem");
            }

            //Como recebe o DTO, preciso criar um objeto que tenha realacao com o BD com os dados completo 
            var novoVinculo = new VinculoProjeto
            {
                //Compilador reclamando que pode ser null !
                Usuario = usuario!,
                Projeto = projeto!,
                Funcao = vinculo.Funcao
            };
            
            _context.Vinculos.Add(novoVinculo);
            await _context.SaveChangesAsync();
            //gera o id da entidade completa
            return CreatedAtAction(nameof(GetVinculo), new { id = novoVinculo.Id }, novoVinculo);
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
