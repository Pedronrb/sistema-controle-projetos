namespace SistemaDeControle.Server.Models
{
    public class VinculoProjeto
    {
        public int ProjetoId { get; set; }
        public int UsuarioId { get; set; }
        public Funcao Funcao { get; set; } // Mantenha como enum, não como string

        public Usuario? Usuario { get; set; }
        public Projeto? Projeto { get; set; }
    }
}
