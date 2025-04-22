using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SistemaDeControle.Server;
using SistemaDeControle.Server.Models;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using SistemaDeControle.Server.Data; // Necessário para acessar o AppDbContext

namespace SistemaDeControle.Tests.TesteController
{
    public class UsuarioControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public UsuarioControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        // Teste para o GET: api/usuarios
        [Fact]
        public async Task Get_Usuarios_ReturnsSuccessStatusCode()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/usuarios");

            // Assert
            response.EnsureSuccessStatusCode(); // Verifica se o código de status é 200-299
            Assert.Equal("application/json; charset=utf-8", response.Content?.Headers?.ContentType?.ToString()); // Verifica o tipo de conteúdo da resposta
        }

        // Teste para o GET com ID: api/usuarios/{id}
        [Fact]
        public async Task GetById_Usuario_ReturnsSuccessStatusCode()
        {
            // Arrange
            var client = _factory.CreateClient();
            var usuarioId = 1; // ID de um usuário válido no banco de dados para o teste

            // Act
            var response = await client.GetAsync($"/api/usuarios/{usuarioId}");

            // Assert
            response.EnsureSuccessStatusCode(); // Verifica se o código de status é 200-299
        }

        // Teste para o POST: api/usuarios
        [Fact]
        public async Task Post_Usuario_ReturnsCreatedStatusCode()
        {
            // Arrange
            var client = _factory.CreateClient();
            var usuario = new Usuario
            {
                Nome = "Teste",
                Email = "usuario@teste.com",
                Senha = "123456",
                Tipo = TipoUsuario.Aluno
            };

            var json = JsonConvert.SerializeObject(usuario); // Serializa o objeto para JSON
            var content = new StringContent(json, Encoding.UTF8, "application/json"); // Prepara o conteúdo para envio

            // Act
            var response = await client.PostAsync("/api/usuarios", content);

            // Assert
            response.EnsureSuccessStatusCode(); // Verifica se o código de status é 200-299
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode); // Verifica se o código de status é 201 Created
        }

        // Teste que utiliza o IServiceScopeFactory para obter o DbContext
        [Fact]
        public void TestWithServiceScope()
        {
            // Arrange
            var scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>(); // Obtém a fábrica de escopo de serviços
            using (var scope = scopeFactory.CreateScope()) // Cria um escopo para resolver os serviços
            {
                var serviceProvider = scope.ServiceProvider;
                var dbContext = serviceProvider.GetRequiredService<AppDbContext>(); // Resolve o DbContext

                // Assert
                Assert.NotNull(dbContext); // Verifica se o DbContext foi resolvido corretamente
            }
        }
    }
}
