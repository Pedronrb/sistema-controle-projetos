using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using SistemaDeControle.Server.Data;
using SistemaDeControle.Server.Models;
using SistemaDeControle.Server.Services;

namespace SistemaDeControle.Tests
{
    public class UsuarioServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task ValidarUsuarioAsync()
        {
            var dbContext = GetInMemoryDbContext();
            var service = new UsuarioService(dbContext);

            var usuario = new Usuario
            {
                Nome = "ProfTeste",
                Email = "prof@teste.com",
                Senha = "123456",
                Tipo = TipoUsuario.Professor,
                AreaAtuacao = "",
                Formacao = null
            };

            var (isValid, error) = await service.ValidarUsuarioAsync(usuario);

            Assert.False(isValid);
            Assert.Equal("Atuação e formação são obrigatórias para professores.", error);
        }

        [Fact]
        public async Task CriarUsuarioAsync()
        {
            var dbContext = GetInMemoryDbContext();
            var service = new UsuarioService(dbContext);

            var usuario = new Usuario
            {
                Nome = "Teste",
                Email = "usuario@teste.com",
                Senha = "123",
                Tipo = TipoUsuario.Aluno
            };

            var criado = await service.CriarUsuarioAsync(usuario);

            var buscado = await dbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == "usuario@teste.com");

            Assert.NotNull(buscado);
            Assert.Equal("Teste", buscado!.Nome);
        }

        [Fact]
        public async Task ListarUsuariosAsync()
        {
            var dbContext = GetInMemoryDbContext();
            dbContext.Usuarios.AddRange(new[]
            {
                new Usuario { Nome = "Usuário 1", Email = "u1@example.com", Senha = "123", Tipo = TipoUsuario.Aluno },
                new Usuario { Nome = "Usuário 2", Email = "u2@example.com", Senha = "123", Tipo = TipoUsuario.Professor, AreaAtuacao = "TI", Formacao = "Mestrado" }
            });
            await dbContext.SaveChangesAsync();

            var service = new UsuarioService(dbContext);
            var usuarios = await service.ListarUsuariosAsync();

            Assert.Equal(2, usuarios.Count);
        }

        [Fact]
        public async Task BuscarPorIdAsync()
        {
            var dbContext = GetInMemoryDbContext();
            var usuario = new Usuario { Nome = "Pedro", Email = "pedro@example.com", Senha = "123", Tipo = TipoUsuario.Aluno };
            dbContext.Usuarios.Add(usuario);
            await dbContext.SaveChangesAsync();

            var service = new UsuarioService(dbContext);
            var encontrado = await service.BuscarPorIdAsync(usuario.Id);

            Assert.NotNull(encontrado);
            Assert.Equal("Pedro", encontrado!.Nome);
        }

        [Fact]
        public async Task BuscarPorIdInexistenteAsync()
        {
            var dbContext = GetInMemoryDbContext();
            var service = new UsuarioService(dbContext);

            var resultado = await service.BuscarPorIdAsync(999);

            Assert.Null(resultado);
        }
    }
}
