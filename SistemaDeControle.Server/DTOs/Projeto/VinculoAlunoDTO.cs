using SistemaDeControle.Server.Models;  // Adicione esta linha para importar o enum Funcao

namespace SistemaDeControle.Server.DTOs.Projeto
{
    public class VinculoAlunoDTO
    {
        public int AlunoId { get; set; }
        public Funcao Funcao { get; set; } // Mantenha como enum, não como string
    }
}
