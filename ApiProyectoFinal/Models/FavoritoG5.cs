using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProyectoFinal.Models
{
    public class FavoritoG5
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Favorito { get; set; }

        [Required]
        public int ID_Usuario { get; set; }

        [ForeignKey("ID_Usuario")]
        public UsuarioG5 Usuario { get; set; }

        [Required]
        public int ID_Libro { get; set; }

        [ForeignKey("ID_Libro")]
        public LibroG5 Libro { get; set; }

        public DateTime Fecha_Agregado { get; set; } = DateTime.UtcNow;
    }
}