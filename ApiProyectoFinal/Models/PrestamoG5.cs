using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProyectoFinal.Models
{
    public class PrestamoG5
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Prestamo { get; set; }

        [Required]
        public int ID_Usuario { get; set; }

        [ForeignKey("ID_Usuario")]
        public UsuarioG5 Usuario { get; set; }

        [Required]
        public int ID_Libro { get; set; }

        [ForeignKey("ID_Libro")]
        public LibroG5 Libro { get; set; }

        [Required]
        public DateTime Fecha_Prestamo { get; set; }

        public DateTime? Fecha_Devolucion { get; set; }

        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } // 'Prestado', 'Devuelto', 'Atrasado'
    }
}