using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace PrograAvanBiblioteca.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Eliminar todo lo de la sesión
            HttpContext.Session.Clear();

            // Redirigir al login
            return RedirectToPage("/Login");
        }
    }
}
