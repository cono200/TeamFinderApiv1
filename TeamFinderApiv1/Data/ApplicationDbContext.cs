using Microsoft.EntityFrameworkCore;
using TeamFinder.API.Models; // Importamos nuestros modelos

namespace TeamFinder.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        // El constructor es necesario para que la inyección de dependencias de ASP.NET Core funcione.
        // Recibe las opciones de configuración (como la cadena de conexión).
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Cada DbSet representa una tabla en la base de datos.
        // El nombre de la propiedad "Usuarios" será el nombre de la tabla (o se puede configurar).
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Juego> Juegos { get; set; }
        public DbSet<UsuarioJuego> UsuarioJuegos { get; set; }
        public DbSet<Like> Likes { get; set; }

        // Este método se usa para configurar el modelo de forma más avanzada.
        // Aquí definiremos la clave primaria compuesta para nuestra tabla intermedia.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración para la tabla UsuarioJuego
            // Le decimos que su clave primaria es la combinación de UsuarioId y JuegoId.
            modelBuilder.Entity<UsuarioJuego>()
                .HasKey(uj => new { uj.UsuarioId, uj.JuegoId });

            // Configuración para evitar que un usuario se dé like a sí mismo.
            // Esto es una regla de negocio a nivel de base de datos.
            modelBuilder.Entity<Like>()
                .HasOne(l => l.UsuarioQueDaLike)
                .WithMany(u => u.LikesDados)
                .HasForeignKey(l => l.UsuarioQueDaLikeId)
                .OnDelete(DeleteBehavior.Restrict); // Evita borrado en cascada para prevenir errores

            modelBuilder.Entity<Like>()
                .HasOne(l => l.UsuarioRecibeLike)
                .WithMany(u => u.LikesRecibidos)
                .HasForeignKey(l => l.UsuarioRecibeLikeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}