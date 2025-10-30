using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamFinder.API.Models
{
    // Esta clase representa la tabla que une Usuarios y Juegos.
    public class UsuarioJuego
    {
        // La clave primaria de esta tabla será la combinación de UsuarioId y JuegoId.
        // La configuraremos en el DbContext más adelante.
        public int UsuarioId { get; set; }
        public int JuegoId { get; set; }

        // Propiedades de navegación para acceder a los objetos completos.
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        [ForeignKey("JuegoId")]
        public virtual Juego Juego { get; set; }
    }
}