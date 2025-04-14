using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaDeControle.Server.Data;
using SistemaDeControle.Server.Models;

namespace SistemaDeControle.Server.Services
{
	public class UsuarioService
	{
		private readonly AppDbContext _context;

		public UsuarioService(AppDbContext context)
		{
			_context = context;
		}

		public Task<(bool IsValid, string? ErrorMessage)> ValidarUsuarioAsync(Usuario usuario)
		{
			if (usuario.Tipo == TipoUsuario.Professor)
			{
				if (string.IsNullOrWhiteSpace(usuario.AreaAtuacao) || string.IsNullOrWhiteSpace(usuario.Formacao))
				{
					return Task.FromResult<(bool, string?)>((false, "Atuação e formação são obrigatórias para professores."));
				}
			}

			return Task.FromResult<(bool, string?)>((true, null));
		}

		public async Task<Usuario> CriarUsuarioAsync(Usuario usuario)
		{
			_context.Usuarios.Add(usuario);
			await _context.SaveChangesAsync();
			return usuario;
		}

		public async Task<List<Usuario>> ListarUsuariosAsync()
		{
			return await _context.Usuarios.ToListAsync();
		}

		public async Task<Usuario?> BuscarPorIdAsync(int id)
		{
			return await _context.Usuarios.FindAsync(id);
		}
	}
}
