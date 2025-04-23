using SistemaDeControle.Server.Models;
namespace SistemaDeControle.Server.DTOs
{
	public class UsuarioCriacaoDTO
	{
		public string Nome { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Senha { get; set; } = string.Empty;
		public TipoUsuario Tipo { get; set; }

		// Só para professores
		public string? AreaAtuacao { get; set; }
		public string? Formacao { get; set; }
	}
}
