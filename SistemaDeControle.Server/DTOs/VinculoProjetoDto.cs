namespace SistemaDeControle.Server.DTOs
{
    public class VinculoProjetoDTO {
        public int UsuarioId { get; set; }
        public int ProjetoId { get; set; }
        public string Funcao { get; set; } = string.Empty;

    }

}