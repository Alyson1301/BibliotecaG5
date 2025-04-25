using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiProyectoFinal.Models
{
    [Table("FIDE_AUTORESG5")]
    public class AutorG5
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_Autor")]
        public int ID_Autor { get; set; }

        [Required]
        [Column("Nombre")]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [Column("Nacionalidad")]
        [MaxLength(50)]
        public string Nacionalidad { get; set; }

        // Colección de libros asociados a este autor
        [JsonIgnore]
        public virtual ICollection<LibroG5> Libros { get; set; } = new List<LibroG5>();
    }
}