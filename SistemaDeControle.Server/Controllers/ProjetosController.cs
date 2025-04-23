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
    [Authorize]  // Apenas usuários autenticados
    public class ProjetosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjetosController(AppDbContext context)
        {
            _context = context;
        }

        // Endpoint para criação de projeto
        [HttpPost]
        public async Task<IActionResult> CriarProjeto([FromBody] ProjetoCriacaoDTO projetoCriacaoDTO)
        {
            var usuarioIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdString) || !int.TryParse(usuarioIdString, out var usuarioId))
            {
                return Unauthorized("Não foi possível identificar o usuário.");
            }

            var usuario = await _context.Usuarios.FindAsync(usuarioId);

            if (usuario == null || usuario.Tipo != TipoUsuario.Professor)
            {
                return Unauthorized("Apenas professores podem criar projetos.");
            }

            // Criação do projeto
            var projeto = new Projeto
            {
                Nome = projetoCriacaoDTO.Nome,
                Descricao = projetoCriacaoDTO.Descricao,
            };

            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync();

            
            var vinculo = new VinculoProjeto
            {
                UsuarioId = usuario.Id,
                ProjetoId = projeto.Id,
                Funcao = Funcao.Master 
            };

            _context.Vinculos.Add(vinculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CriarProjeto), new { id = projeto.Id }, projeto);
        }

       
        [HttpGet]
        public async Task<IActionResult> ListarProjetos()
        {
            var projetos = await _context.Projetos.ToListAsync();
            return Ok(projetos);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> DetalharProjeto(int id)
        {
            var projeto = await _context.Projetos
                .Include(p => p.Vinculos)
                .ThenInclude(v => v.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (projeto == null)
                return NotFound("Projeto não encontrado.");

            var projetoDTO = new ProjetoDTO
            {
                Id = projeto.Id,
                Nome = projeto.Nome,
                Descricao = projeto.Descricao,
                Vinculos = projeto.Vinculos.Select(v => new VinculoDTO
                {
                    AlunoNome = v.Usuario?.Nome ?? "Nome não disponível",
                    Funcao = v.Funcao
                }).ToList()
            };

            return Ok(projetoDTO);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarProjeto(int id, [FromBody] ProjetoDTO projetoDTO)
        {
            var projeto = await _context.Projetos.FindAsync(id);
            if (projeto == null)
                return NotFound("Projeto não encontrado.");

            projeto.Nome = projetoDTO.Nome;
            projeto.Descricao = projetoDTO.Descricao;

            _context.Projetos.Update(projeto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        
        [HttpPost("{projetoId}/vincular-aluno")]
        public async Task<IActionResult> VincularAluno(int projetoId, [FromBody] VinculoAlunoDTO vinculoDTO)
        {
            var projeto = await _context.Projetos.FindAsync(projetoId);
            if (projeto == null)
                return NotFound("Projeto não encontrado.");

            var usuario = await _context.Usuarios.FindAsync(vinculoDTO.AlunoId);
            if (usuario == null || usuario.Tipo != TipoUsuario.Aluno)
                return BadRequest("Usuário não encontrado ou tipo de usuário inválido.");

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

      
        [HttpDelete("{projetoId}/desvincular-aluno/{alunoId}")]
        public async Task<IActionResult> DesvincularAluno(int projetoId, int alunoId)
        {
            var projeto = await _context.Projetos.FindAsync(projetoId);
            if (projeto == null)
                return NotFound("Projeto não encontrado.");

            var vinculo = await _context.Vinculos
                .FirstOrDefaultAsync(v => v.ProjetoId == projetoId && v.UsuarioId == alunoId);

            if (vinculo == null)
                return BadRequest("Aluno não vinculado a esse projeto.");

            _context.Vinculos.Remove(vinculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        
        [HttpPost("{projetoId}/promover/{usuarioId}")]
        public async Task<IActionResult> PromoverUsuario(int projetoId, int usuarioId, [FromBody] string novaFuncao)
        {
            
            var projeto = await _context.Projetos.FindAsync(projetoId);
            if (projeto == null)
                return NotFound("Projeto não encontrado.");

            
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
                return BadRequest("Usuário não encontrado.");

            
            var vinculo = await _context.Vinculos
                .FirstOrDefaultAsync(v => v.ProjetoId == projetoId && v.UsuarioId == usuarioId);

            if (vinculo == null)
                return BadRequest("Usuário não está vinculado a esse projeto.");

           
            var usuarioIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(usuarioIdString) || !int.TryParse(usuarioIdString, out var usuarioIdLogado))
            {
                return Unauthorized("Não foi possível identificar o usuário.");
            }

            // Verifica se o coordenador logado é o responsável pelo projeto (Master)
            var vinculoProfessor = await _context.Vinculos
                .FirstOrDefaultAsync(v => v.ProjetoId == projetoId && v.UsuarioId == usuarioIdLogado && v.Funcao == Funcao.Master);

            if (vinculoProfessor == null)
            {
                return Unauthorized("Somente o coordenador do projeto pode promover usuários.");
            }

            
            if (novaFuncao.ToUpper() == Funcao.Master.ToString())
            {
                
                var masterExistente = await _context.Vinculos
                    .FirstOrDefaultAsync(v => v.ProjetoId == projetoId && v.Funcao == Funcao.Master);

                if (masterExistente != null)
                {
                    return BadRequest("Já existe um Master ");
                }

                
                vinculo.Funcao = Funcao.Master;
            }
            
            else if (novaFuncao.ToUpper() == Funcao.Senior.ToString())
            {
                
                vinculo.Funcao = Funcao.Senior;
            }
            
            else if (novaFuncao.ToUpper() == Funcao.Junior.ToString())
            {
                
                vinculo.Funcao = Funcao.Junior;
            }
            
            else if (novaFuncao.ToUpper() == Funcao.Estagiario.ToString())
            {
               
                vinculo.Funcao = Funcao.Estagiario;
            }
            else
            {
                return BadRequest("Função inválida.");
            }

           
            _context.Vinculos.Update(vinculo);
            await _context.SaveChangesAsync();

            return Ok("Usuário promovido com sucesso.");
        }
    }
}
