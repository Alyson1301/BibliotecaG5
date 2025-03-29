using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProyectoFinal.Models
{
    public class LibroG5
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Libro { get; set; }

        [Required]
        [MaxLength(255)]
        public string Titulo { get; set; }

        [Required]
        public int ID_Autor { get; set; }

        [ForeignKey("ID_Autor")]
        public AutorG5 Autor { get; set; }

        public int? ID_Categoria { get; set; }

        [ForeignKey("ID_Categoria")]
        public CategoriaG5 Categoria { get; set; }

        [Required]
        [MaxLength(20)]
        public string ISBN { get; set; }

        [Required]
        public int Año_Publicación { get; set; }

        // Propiedades de navegación
        public ICollection<FavoritoG5> Favoritos { get; set; }
        public ICollection<DisponibilidadG5> Disponibilidades { get; set; }
        public ICollection<PrestamoG5> Prestamos { get; set; }
        public ICollection<FeedbackG5> Feedbacks { get; set; }
        public ICollection<TransaccionG5> Transacciones { get; set; }
    }
}