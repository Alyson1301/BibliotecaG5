using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProyectoFinal.Models
{
    public class CategoriaG5
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Categoria { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }
    }
}
