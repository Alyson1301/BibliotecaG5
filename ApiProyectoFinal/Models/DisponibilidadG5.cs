using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProyectoFinal.Models
{
    public class DisponibilidadG5
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Disponibilidad { get; set; }

        [Required]
        public int ID_Libro { get; set; }

        [ForeignKey("ID_Libro")]
        public LibroG5 Libro { get; set; }

        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } // 'Disponible', 'Prestado', 'Reservado'
    }
}