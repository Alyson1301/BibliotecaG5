using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProyectoFinal.Models
{
    public class TransaccionG5
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Transaccion { get; set; }

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

        [Required]
        public DateTime Fecha_Devolucion_Esperada { get; set; }

        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } // 'Prestado', 'Devuelto', 'Atrasado'
    }
}