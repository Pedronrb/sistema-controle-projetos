namespace sistemadecontrole.Server.Models
{
    public class Projeto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public int CoordenadorId { get; set; }
        public Usuario Coordenador { get; set; }

        public ICollection<VinculoProjeto> Vinculos { get; set; }
    }
}
