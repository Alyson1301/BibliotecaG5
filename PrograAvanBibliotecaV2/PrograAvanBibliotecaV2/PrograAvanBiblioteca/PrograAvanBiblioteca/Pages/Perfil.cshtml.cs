using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PrograAvanBiblioteca.Pages
{
    public class PerfilModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PerfilModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public UsuarioG5 Usuario { get; set; }

        [BindProperty]
        public string adminPassword { get; set; } // campo para la contraseña adicional

        [BindProperty(SupportsGet = true)]
        public string Mensaje { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var idString = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(idString) || !int.TryParse(idString, out int id))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7058/api/Usuarios/{id}");

            if (!response.IsSuccessStatusCode)
            {
                Mensaje = "Error al obtener datos del perfil.";
                return Page();
            }

            var json = await response.Content.ReadAsStringAsync();
            Usuario = JsonConvert.DeserializeObject<UsuarioG5>(json);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var idString = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(idString) || !int.TryParse(idString, out int id))
            {
                return RedirectToPage("/Login");
            }

            // Si quiere cambiar a administrador, validar contraseña
            if (Usuario.Rol == "Administrador" && adminPassword != "1210")
            {
                Mensaje = "Contraseña de administrador incorrecta. No se aplicaron los cambios.";
                return await OnGetAsync();
            }

            Usuario.ID_Usuario = id;

            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(Usuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"https://localhost:7058/api/Usuarios/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                Mensaje = "Perfil actualizado correctamente.";
            }
            else
            {
                Mensaje = "Error al actualizar perfil.";
            }

            return await OnGetAsync();
        }
    }

    public class UsuarioG5
    {
        public int ID_Usuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public string Rol { get; set; }
    }
}
