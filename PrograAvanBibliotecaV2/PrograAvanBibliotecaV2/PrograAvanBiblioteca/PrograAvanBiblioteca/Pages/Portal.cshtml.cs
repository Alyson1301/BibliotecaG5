using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace PrograAvanBiblioteca.Pages
{
    public class PortalModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PortalModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<UsuarioG5> Usuarios { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string rolFiltro)
        {
            var rol = HttpContext.Session.GetString("Rol");

            if (rol != "Administrador")
                return RedirectToPage("/Index");

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7058/api/Usuarios");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                Usuarios = JsonSerializer.Deserialize<List<UsuarioG5>>(json, options) ?? new List<UsuarioG5>();

                // Filtrar los usuarios por el rol seleccionado
                if (!string.IsNullOrEmpty(rolFiltro))
                {
                    Usuarios = Usuarios.Where(u => u.Rol == rolFiltro).ToList();
                }
            }

            return Page();
        }
    }
}
