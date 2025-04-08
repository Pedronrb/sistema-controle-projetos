namespace sistemadecontrole.Server.Models
{
    public class Projeto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;

        public int CoordenadorId { get; set; }
        //Quem criou o projeto e o cordenador! e e do tipo Usuario!!
        //Ta reclamando q tem que inicializar coordenador!!
        public Usuario Coordenador { get; set; } = null!;
        //Erro p q estava inicializando diretamente uma interface e n pode. 
        public ICollection<VinculoProjeto> Vinculos { get; set; } = new List<VinculoProjeto>();
    }
}
