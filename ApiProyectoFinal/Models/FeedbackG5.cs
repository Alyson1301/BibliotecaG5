using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProyectoFinal.Models
{
    public class FeedbackG5
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Feedback { get; set; }

        [Required]
        public int ID_Usuario { get; set; }

        [ForeignKey("ID_Usuario")]
        public UsuarioG5 Usuario { get; set; }

        [Required]
        public int ID_Libro { get; set; }

        [ForeignKey("ID_Libro")]
        public LibroG5 Libro { get; set; }

        [Required]
        public string Comentario { get; set; }

        [Required]
        [Range(1, 5)]
        public int Puntuacion { get; set; }

        public DateTime Fecha_Comentario { get; set; } = DateTime.UtcNow;
    }
}