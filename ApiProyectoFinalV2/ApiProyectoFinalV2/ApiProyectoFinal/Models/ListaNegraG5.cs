using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProyectoFinal.Models
{
    public class ListaNegraG5
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Lista_Negra { get; set; }

        [Required]
        public int ID_Usuario { get; set; }

        [ForeignKey("ID_Usuario")]
        public UsuarioG5 Usuario { get; set; }

        [Required]
        public DateTime Fecha_Agregado { get; set; } = DateTime.UtcNow;

        [Required]
        public string Motivo { get; set; }

        [Required]
        public DateTime Fecha_Fin_Restriccion { get; set; }
    }
}