using SistemaDeControle.Server.Models;
using SistemaDeControle.Server.DTOs.Projeto;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using SistemaDeControle.Server.Data;

namespace SistemaDeControle.Server.Services
{
    public class ProjetoService
    {
        private readonly AppDbContext _context;

        public ProjetoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task VincularAlunoAsync(int projetoId, VinculoAlunoDTO dto, int usuarioId)
        {
            var projeto = await _context.Projetos
                .FirstOrDefaultAsync(p => p.Id == projetoId && p.Vinculos.Any(v => v.UsuarioId == usuarioId));

            if (projeto == null)
            {
                throw new InvalidOperationException("Projeto não encontrado ou acesso negado.");
            }

            var aluno = await _context.Usuarios.FindAsync(dto.AlunoId);
            if (aluno == null || aluno.Tipo != TipoUsuario.Aluno)
            {
                throw new InvalidOperationException("Aluno não encontrado ou tipo de usuário inválido.");
            }

            // Conversão da função de string para enum Funcao de forma segura
            if (!Enum.TryParse<Funcao>(dto.Funcao.ToString(), out var funcaoEnum))
            {
                throw new InvalidOperationException("Função inválida.");
            }

            var vinculo = new VinculoProjeto
            {
                ProjetoId = projetoId,
                UsuarioId = dto.AlunoId,
                Funcao = funcaoEnum // Atribuindo diretamente o valor do enum
            };

            _context.Vinculos.Add(vinculo);
            await _context.SaveChangesAsync();
        }

        public async Task<ProjetoDTO> DetalharProjetoAsync(int projetoId)
        {
            var projeto = await _context.Projetos
                .Include(p => p.Vinculos)
                .ThenInclude(v => v.Usuario)
                .FirstOrDefaultAsync(p => p.Id == projetoId);

            if (projeto == null)
                throw new InvalidOperationException("Projeto não encontrado.");

            var projetoDTO = new ProjetoDTO
            {
                Id = projeto.Id,
                Nome = projeto.Nome,
                Descricao = projeto.Descricao,
                Vinculos = projeto.Vinculos.Select(v => new VinculoDTO
                {
                    AlunoNome = v.Usuario?.Nome ?? "Desconhecido", // <-- Aqui é onde evitamos o warning
                    Funcao = v.Funcao
                }).ToList()
            };

            return projetoDTO;
        }
    }
}
