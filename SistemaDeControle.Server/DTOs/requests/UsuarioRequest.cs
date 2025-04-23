namespace SistemaDeControle.Server.DTOs
{
    public class UsuarioRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}