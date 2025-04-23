
using System;
using System.Linq;
using SistemaDeControle.Server.Data;
using SistemaDeControle.Server.Models;

namespace sistemadecontrole.Server.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public bool VerificarUsuario(string email, string senha, string tipo)
        {
            var user = _context.Usuarios.FirstOrDefault(u =>
                u.Email == email &&
                u.Tipo.ToString().Equals(tipo, StringComparison.OrdinalIgnoreCase));

            if (user == null) return false;

            return BCrypt.Net.BCrypt.Verify(senha, user.Senha);
        }

        public Usuario? ObterUsuario(string email)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Email == email);
        }
    }
}
