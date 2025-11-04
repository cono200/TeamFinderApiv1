using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamFinder.API.Data; // Importamos el DbContext
using TeamFinder.API.Models; // Importamos los Models

namespace TeamFinder.API.Controllers
{
    [Route("api/[controller]")] // Define la ruta base: /api/usuarios
    [ApiController] // Indica que es un controlador de API
    public class UsuariosController : ControllerBase
    {
        // 1. Inyección de Dependencias del DbContext
        // El ApplicationDbContext se inyecta automáticamente, gracias a que lo registramos en Program.cs
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 2. ENDPOINT PARA OBTENER TODOS LOS USUARIOS
        // GET: api/usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            // Obtiene la lista de usuarios de la base de datos de forma asíncrona
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios); // Devuelve un 200 OK con la lista de usuarios en formato JSON
        }

        // 3. ENDPOINT PARA OBTENER UN USUARIO POR SU ID
        // GET: api/usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            // Busca un usuario por su ID
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound(); // Devuelve un 404 Not Found si no existe
            }

            //SIMULANDO UN CAMBIO EN EL CODIGO 
            var resultado = usuario;
            return Ok(resultado); // Devuelve un 200 OK con el usuario encontrado
        }

        // 4. ENDPOINT PARA CREAR UN NUEVO USUARIO
        // POST: api/usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> CrearUsuario(Usuario usuario)
        {
            // Añade el nuevo usuario al contexto
            _context.Usuarios.Add(usuario);

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Devuelve una respuesta 201 Created, que es la correcta para una creación.
            // También añade una cabecera 'Location' con la URL del nuevo recurso.
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        // 5. ENDPOINT PERSONALIZADO PARA LA ACTIVIDAD (Descubrir Usuarios)
        // GET: api/usuarios/descubrir/5
        [HttpGet("descubrir/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<Usuario>>> DescubrirUsuarios(int usuarioId)
        {
            // Lógica simple: devuelve todos los usuarios excepto el que hace la petición.
            // En una app real, aquí iría el algoritmo de matching.
            var usuariosParaDescubrir = await _context.Usuarios
                .Where(u => u.Id != usuarioId)
                .ToListAsync();

            return Ok(usuariosParaDescubrir);
        }

        [HttpDelete("{usuarioId}/juegos/{juegoId}")]
        public async Task<IActionResult> QuitarJuegoFavorito(int usuarioId, int juegoId)
        {
            var usuarioJuego = await _context.UsuarioJuegos.FirstOrDefaultAsync(uj =>
                uj.UsuarioId == usuarioId && uj.JuegoId == juegoId);

            if (usuarioJuego == null)
            {
                return NotFound("La relación de juego favorito no existe.");
            }

            _context.UsuarioJuegos.Remove(usuarioJuego);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Juego quitado de favoritos." });
        }
    }
}