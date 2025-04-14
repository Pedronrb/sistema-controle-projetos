namespace SistemaDeControle.Server.Models
{

    //So professor pode fazer vinculacao a paritr do projeto como coordenador.
    public class VinculoProjeto
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        //Usuario e uma propri. de navegacao, representa os relacionamentos entre as entidades
        public Usuario Usuario { get; set; } = null!;

        public int ProjetoId { get; set; }
        public Projeto Projeto { get; set; } = null!;

        public string Funcao { get; set; } = string.Empty;
    }

}
