using System.ComponentModel.DataAnnotations.Schema;

namespace sistemadecontrole.Server.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Tipo { get; set; } // "aluno" ou "professor"

        // Apenas para professores
        public string? AreaAtuacao { get; set; }
        public string? Formacao { get; set; }

        public ICollection<Projeto>? ProjetosCoordenados { get; set; }
        public ICollection<VinculoProjeto>? ProjetosParticipando { get; set; }
    }
}
