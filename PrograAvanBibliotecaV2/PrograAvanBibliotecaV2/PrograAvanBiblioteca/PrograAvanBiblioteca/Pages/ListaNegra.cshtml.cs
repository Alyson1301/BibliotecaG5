using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PrograAvanBiblioteca.Models;
using System.Collections.Generic;
using System.Linq;

namespace PrograAvanBiblioteca.Pages.ListaNegra
{
    public class ListaNegraModel : PageModel
    {
        public List<Models.ListaNegra> UsuariosEnListaNegra { get; set; }

        public void OnGet()
        {
            UsuariosEnListaNegra = ObtenerListaNegra();
        }

        public IActionResult OnPostResolver(int id)
        {
            var usuarios = ObtenerListaNegra(); // Simulación temporal
            var usuario = usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario != null)
            {
                usuario.Activo = false;
                // Aquí podrías guardar el cambio si se usara BD
            }

            return RedirectToPage();
        }

        private List<Models.ListaNegra> ObtenerListaNegra()
        {
            return new List<Models.ListaNegra>
            {
                new Models.ListaNegra { Id = 1, UsuarioId = 1, NombreUsuario = "Alyson Olivas", Motivo = "No devolvió el libro", FechaRegistro = DateTime.Today.AddDays(-5), Activo = true },
                new Models.ListaNegra { Id = 2, UsuarioId = 2, NombreUsuario = "Carlos Mena", Motivo = "Libro dañado", FechaRegistro = DateTime.Today.AddDays(-10), Activo = false }
            };
        }
    }
}
