using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProyectoFinal.Models
{
    [Table("FIDE_DISPONIBILIDADG5")]
    public class DisponibilidadG5
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_Disponibilidad")]
        public int ID_Disponibilidad { get; set; }

        [Required]
        [Column("ID_Libro")]
        public int ID_Libro { get; set; }

        [Required]
        [Column("Estado")]
        [MaxLength(50)]
        public string Estado { get; set; } = "Disponible";

        // Relación con Libro
        [ForeignKey("ID_Libro")]
        public virtual LibroG5 Libro { get; set; }
    }
}