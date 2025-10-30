using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamFinder.API.Models
{
    public class Juego
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; }

        public string Genero { get; set; } // Ej: FPS, RPG, MOBA

        // Un juego puede ser favorito de muchos usuarios.
        // Esta es la otra parte de la relación "muchos a muchos".
        public virtual ICollection<UsuarioJuego> Jugadores { get; set; }
    }
}