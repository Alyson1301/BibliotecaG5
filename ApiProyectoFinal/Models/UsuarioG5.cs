using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProyectoFinal.Models
{
    public class UsuarioG5
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Usuario { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(100)]
        public string Apellido { get; set; }

        [Required]
        [MaxLength(255)]
        public string Correo { get; set; }

        [Required]
        [MaxLength(255)]
        public string Contraseña { get; set; }

        [Required]
        [MaxLength(20)]
        public string Rol { get; set; }

        // Propiedades de navegación
        public ICollection<FavoritoG5> Favoritos { get; set; }
        public ICollection<PrestamoG5> Prestamos { get; set; }
        public ICollection<FeedbackG5> Feedbacks { get; set; }
        public ICollection<TransaccionG5> Transacciones { get; set; }
        public ICollection<ListaNegraG5> ListaNegra { get; set; }
    }
}