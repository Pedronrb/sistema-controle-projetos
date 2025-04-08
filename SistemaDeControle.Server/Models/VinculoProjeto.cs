namespace sistemadecontrole.Server.Models
{
    public class VinculoProjeto
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int ProjetoId { get; set; }
        public Projeto Projeto { get; set; }

        public string Funcao { get; set; } // Estagiário, Júnior, Sênior, Master
    }
}
