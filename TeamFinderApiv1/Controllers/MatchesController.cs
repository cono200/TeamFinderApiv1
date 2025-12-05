using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamFinder.API.Data;
using TeamFinder.API.Models;

namespace TeamFinder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MatchesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/matches/like
        // Registra que un usuario le da like a otro y comprueba si es un match.
        // El cuerpo de la petición (body) será un objeto JSON como: { "usuarioQueDaLikeId": 1, "usuarioRecibeLikeId": 2 }
        [HttpPost("like")]
        public async Task<IActionResult> DarLike([FromBody] Like likeRequest)
        {
            // Verificar si ya existe este like para evitar duplicados
            var likeExistente = await _context.Likes.AnyAsync(l =>
                l.UsuarioQueDaLikeId == likeRequest.UsuarioQueDaLikeId &&
                l.UsuarioRecibeLikeId == likeRequest.UsuarioRecibeLikeId);

            if (likeExistente)
            {
                return BadRequest(new { mensaje = "Ya le has dado like a este usuario......" });
            }

            // Crear y guardar el nuevo like
            _context.Likes.Add(likeRequest);
            await _context.SaveChangesAsync();

            // Comprobar si hay un match mutuo
            bool esMatch = await _context.Likes.AnyAsync(l =>
                l.UsuarioQueDaLikeId == likeRequest.UsuarioRecibeLikeId &&
                l.UsuarioRecibeLikeId == likeRequest.UsuarioQueDaLikeId);

            if (esMatch)
            {
                return Ok(new { mensaje = "¡Es un Match!", esMatch = true });
            }

            return Ok(new { mensaje = "Like registrado", esMatch = false });
        }

        // GET: api/matches/{usuarioId}
        // Obtiene la lista de usuarios con los que hay un match mutuo.
        [HttpGet("{usuarioId}")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetMatches(int usuarioId)
        {
            // 1. Obtener la lista de IDs de usuarios a los que el usuario actual les ha dado like.
            var usuariosALosQueLesDioLike = await _context.Likes
                .Where(l => l.UsuarioQueDaLikeId == usuarioId)
                .Select(l => l.UsuarioRecibeLikeId)
                .ToListAsync();

            // 2. De esa lista, encontrar aquellos que también le dieron like al usuario actual.
            var matchIds = await _context.Likes
                .Where(l => usuariosALosQueLesDioLike.Contains(l.UsuarioQueDaLikeId) && l.UsuarioRecibeLikeId == usuarioId)
                .Select(l => l.UsuarioQueDaLikeId)
                .ToListAsync();

            // 3. Obtener la información completa de los usuarios que hicieron match.
            var matches = await _context.Usuarios
                .Where(u => matchIds.Contains(u.Id))
                .ToListAsync();

            return Ok(matches);
        }
    }
}