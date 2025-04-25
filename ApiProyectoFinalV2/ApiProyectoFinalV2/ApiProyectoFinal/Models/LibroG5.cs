using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiProyectoFinal.Models
{
    [Table("FIDE_LIBROSG5")]
    public class LibroG5
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_Libro")]
        public int ID_Libro { get; set; }

        [Required]
        [Column("Titulo")]
        [MaxLength(255)]
        public string Titulo { get; set; }

        [Required]
        [Column("ID_Autor")]
        public int ID_Autor { get; set; }

        [Column("ID_Categoria")]
        public int? ID_Categoria { get; set; }

        [Required]
        [Column("ISBN")]
        [MaxLength(20)]
        public string ISBN { get; set; }

        [Required]
        [Column("Año_Publicación")]
        public int Año_Publicación { get; set; }

        // Propiedad de navegación para Autor
        [ForeignKey("ID_Autor")]
        public virtual AutorG5 Autor { get; set; }

        // Propiedad de navegación para Categoría
        [ForeignKey("ID_Categoria")]
        public virtual CategoriaG5 Categoria { get; set; }

        // Campo para estado de disponibilidad (no mapeado)
        [NotMapped]
        public string EstadoDisponibilidad { get; set; } = "Disponible";
    }
}