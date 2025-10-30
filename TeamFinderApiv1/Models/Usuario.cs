using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamFinder.API.Models
{
    public class Usuario
    {
        // [Key] le dice a Entity Framework que esta propiedad es la clave primaria de la tabla.
        [Key]
        public int Id { get; set; }

        // [Required] indica que este campo es obligatorio.
        [Required]
        [StringLength(100)] // Limita la longitud del texto en la base de datos.
        public string NombreUsuario { get; set; }

        [Required]
        [EmailAddress] // Valida que el texto tenga formato de email.
        public string Email { get; set; }

        // Esta propiedad no se guardará en la base de datos. Es solo para la lógica de la API.
        [NotMapped]
        public string Token { get; set; } // La usaremos en el futuro para autenticación.

        // --- Relaciones con otras tablas ---

        // Un usuario puede tener muchos juegos favoritos.
        // Esta colección representa la relación "muchos a muchos" con la tabla Juegos.
        public virtual ICollection<UsuarioJuego> JuegosFavoritos { get; set; }

        // Un usuario puede dar muchos likes.
        public virtual ICollection<Like> LikesDados { get; set; }

        // Un usuario puede recibir muchos likes.
        public virtual ICollection<Like> LikesRecibidos { get; set; }
    }
}