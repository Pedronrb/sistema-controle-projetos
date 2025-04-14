using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace SistemaDeControle.Server.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        //string.Empty >> para garantir q a inicializacao nao seja nula
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TipoUsuario Tipo { get; set; }
        // Atrb apenas para os professores, faco a verificacao na rota post
        public string? AreaAtuacao { get; set; }
        public string? Formacao { get; set; }

        public ICollection<Projeto>? ProjetosCoordenados { get; set; }
        public ICollection<VinculoProjeto>? ProjetosParticipando { get; set; }
    }
}
