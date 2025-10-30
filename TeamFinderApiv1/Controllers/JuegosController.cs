using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamFinder.API.Data;
using TeamFinder.API.Models;

namespace TeamFinder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JuegosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JuegosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/juegos
        // Obtiene la lista completa de juegos disponibles.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Juego>>> GetJuegos()
        {
            return await _context.Juegos.ToListAsync();
        }

        // GET: api/juegos/5
        // Obtiene un juego específico por su ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<Juego>> GetJuego(int id)
        {
            var juego = await _context.Juegos.FindAsync(id);

            if (juego == null)
            {
                return NotFound();
            }

            return Ok(juego);
        }
    }
}