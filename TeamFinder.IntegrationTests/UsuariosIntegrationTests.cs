using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TeamFinderApiv1;
using TeamFinder.API.Models;

namespace TeamFinder.IntegrationTests
{
    public class UsuariosIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UsuariosIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetUsuarios_DeberiaRetornarOk()
        {
            // Act
            var response = await _client.GetAsync("/api/usuarios");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostUsuario_DeberiaCrearUsuario()
        {
            // Arrange
            var nuevoUsuario = new Usuario
            {
                NombreUsuario = $"Pedro_{Guid.NewGuid()}",
                Email = $"pedro_{Guid.NewGuid()}@test.com",
                
                // Campos adicionales para evitar errores de validación
                Token = Guid.NewGuid().ToString(),
                LikesDados = new List<Like>(),
                LikesRecibidos = new List<Like>(),
                JuegosFavoritos = new List<UsuarioJuego>()
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/usuarios", nuevoUsuario);

            // Si falla, mostrar respuesta detallada en la consola
            if (response.StatusCode != HttpStatusCode.Created)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new Exception($"❌ Error en POST /api/usuarios: {response.StatusCode}\nRespuesta: {body}");
            }

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}

