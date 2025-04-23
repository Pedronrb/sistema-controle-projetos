using System.Collections.Generic;

namespace SistemaDeControle.Server.Models
{
    public class Projeto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;

        // Remover o CoordenadorId e Coordenador, pois o professor que cria o projeto será vinculado através do VinculoProjeto.
        public ICollection<VinculoProjeto> Vinculos { get; set; } = new List<VinculoProjeto>();
    }
}
