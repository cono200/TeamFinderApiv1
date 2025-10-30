using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamFinder.API.Models
{
    public class Like
    {
        [Key]
        public int Id { get; set; }

        public int UsuarioQueDaLikeId { get; set; }
        public int UsuarioRecibeLikeId { get; set; }

        [ForeignKey("UsuarioQueDaLikeId")]
        public virtual Usuario UsuarioQueDaLike { get; set; }

        [ForeignKey("UsuarioRecibeLikeId")]
        public virtual Usuario UsuarioRecibeLike { get; set; }
    }
}