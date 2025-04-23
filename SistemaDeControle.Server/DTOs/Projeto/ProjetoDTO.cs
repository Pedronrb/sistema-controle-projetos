using System.Collections.Generic;
using SistemaDeControle.Server.Models;

namespace SistemaDeControle.Server.DTOs.Projeto
{
    public class ProjetoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;

        public List<VinculoDTO>? Vinculos { get; set; } // s� vem preenchido no detalhamento
    }

    public class VinculoDTO
    {
        public string AlunoNome { get; set; } = string.Empty;
        public Funcao Funcao { get; set; } // Mantenha como enum, n�o como string
    }
}
