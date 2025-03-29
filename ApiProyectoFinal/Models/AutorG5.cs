using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProyectoFinal.Models
{
    public class AutorG5
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Autor { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [MaxLength(100)]
        public string Nacionalidad { get; set; }

        // Propiedad de navegación para los libros escritos por el autor
        public ICollection<LibroG5> Libros { get; set; }
    }
}