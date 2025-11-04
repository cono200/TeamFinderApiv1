using Xunit;
using Microsoft.EntityFrameworkCore;
using TeamFinder.API.Controllers;
using TeamFinder.API.Models;
using TeamFinder.API.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamFinder.Tests
{
    public class UsuariosControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly UsuariosController _controller;

        public UsuariosControllerTests()
        {
            // 🧠 Base de datos única por ejecución (evita conflictos de IDs repetidos)
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"UsuariosTestDb_{Guid.NewGuid()}")
                .Options;

            _context = new ApplicationDbContext(options);

            // Datos de prueba
            _context.Usuarios.AddRange(
                new Usuario { NombreUsuario = "Alex", Email = "alex@mail.com" },
                new Usuario { NombreUsuario = "Sara", Email = "sara@mail.com" }
            );
            _context.SaveChanges();

            _controller = new UsuariosController(_context);
        }

        [Fact]
        public async Task GetUsuarios_ReturnsAllUsuarios()
        {
            // Act
            var actionResult = await _controller.GetUsuarios();

            // Assert
            var result = Assert.IsType<ActionResult<IEnumerable<Usuario>>>(actionResult);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var usuarios = Assert.IsAssignableFrom<IEnumerable<Usuario>>(okResult.Value);

            Assert.Equal(2, usuarios.Count());
        }

        [Fact]
        public async Task GetUsuario_ReturnsUsuario_WhenExists()
        {
            // Act
            var actionResult = await _controller.GetUsuario(1);

            // Assert
            var result = Assert.IsType<ActionResult<Usuario>>(actionResult);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var usuario = Assert.IsType<Usuario>(okResult.Value);

            Assert.Equal("Alex", usuario.NombreUsuario);
        }
    }
}

