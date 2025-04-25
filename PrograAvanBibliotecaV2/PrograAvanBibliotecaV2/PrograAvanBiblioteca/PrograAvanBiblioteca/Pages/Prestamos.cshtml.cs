using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrograAvanBiblioteca.Models;

namespace PrograAvanBiblioteca.Pages.Prestamos
{
    public class NuevoModel : PageModel
    {
        [BindProperty]
        public Prestamo Prestamo { get; set; }

        public List<Prestamo> ListaPrestamos { get; set; }
        public List<UsuarioG5> Usuarios { get; set; }
        public List<Libro> LibrosDisponibles { get; set; }

        public void OnGet()
        {
            ListaPrestamos = ObtenerPrestamos();
            Usuarios = ObtenerUsuarios();
            LibrosDisponibles = ObtenerLibros();
        }

        public IActionResult OnPost()
        {
            var usuario = ObtenerUsuarios().FirstOrDefault(u => u.Id == Prestamo.UsuarioId);
            var libro = ObtenerLibros().FirstOrDefault(l => l.Id == Prestamo.LibroId);

            if (usuario != null && libro != null)
            {
                Prestamo.NombreUsuario = usuario.Nombre;
                Prestamo.NombreLibro = $"{libro.Titulo} - {libro.Autor}";
            }

            // Simulación de guardado
            // Aquí deberías guardar a una base de datos real o usar Session/TempData para persistencia temporal

            return RedirectToPage();
        }

        public IActionResult OnPostActualizarEstado(int id, string nuevoEstado)
        {
            var prestamos = ObtenerPrestamos();
            var prestamo = prestamos.FirstOrDefault(p => p.Id == id);
            if (prestamo != null)
            {
                prestamo.Estado = nuevoEstado;
            }

            return RedirectToPage();
        }

        private List<Prestamo> ObtenerPrestamos()
        {
            return new List<Prestamo>
            {
                new Prestamo { Id = 1, UsuarioId = 1, NombreUsuario = "Alyson Olivas", LibroId = 1, NombreLibro = "1984 - George Orwell", FechaPrestamo = DateTime.Parse("2025-04-24"), FechaDevolucionEstimada = DateTime.Parse("2025-05-01"), Estado = "Pendiente" },
                new Prestamo { Id = 2, UsuarioId = 2, NombreUsuario = "Carlos Mena", LibroId = 2, NombreLibro = "Cien años de soledad - G. G. Márquez", FechaPrestamo = DateTime.Parse("2025-04-20"), FechaDevolucionEstimada = DateTime.Parse("2025-04-27"), Estado = "Entregado" }
            };
        }

        private List<UsuarioG5> ObtenerUsuarios() => new List<UsuarioG5>
        {
            new UsuarioG5 { Id = 1, Nombre = "Alyson Olivas" },
            new UsuarioG5 { Id = 2, Nombre = "Carlos Mena" }
        };

        private List<Libro> ObtenerLibros() => new List<Libro>
        {
            new Libro { Id = 1, Titulo = "1984", Autor = "George Orwell" },
            new Libro { Id = 2, Titulo = "Cien años de soledad", Autor = "Gabriel García Márquez" }
        };
    }
}
