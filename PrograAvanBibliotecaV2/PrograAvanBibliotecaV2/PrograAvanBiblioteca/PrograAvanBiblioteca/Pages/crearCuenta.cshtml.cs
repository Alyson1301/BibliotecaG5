using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PrograAvanBiblioteca.Pages
{
    public class crearCuentaModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public crearCuentaModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        public string Apellido { get; set; } = string.Empty;

        [BindProperty]
        public string Correo { get; set; } = string.Empty;

        [BindProperty]
        public string Contraseña { get; set; } = string.Empty;

        [BindProperty]
        public string Rol { get; set; } = string.Empty;

        [BindProperty]
        public string ClaveAdmin { get; set; } = string.Empty; // <-- nueva propiedad

        public string Mensaje { get; set; } = string.Empty;
        public bool Exito { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            Rol = Rol?.Trim();

            if (Rol != "Estudiante" && Rol != "Administrador")
            {
                Mensaje = "Rol inválido. Debe ser 'Estudiante' o 'Administrador'.";
                Exito = false;
                return Page();
            }

            if (Rol == "Administrador" && ClaveAdmin != "1210")
            {
                Mensaje = "Clave de administrador incorrecta.";
                Exito = false;
                return Page();
            }

            var httpClient = _httpClientFactory.CreateClient();

            var apiUrl = "https://localhost:7058/api/Usuarios";

            var nuevoUsuario = new
            {
                Nombre,
                Apellido,
                Correo,
                Contraseña,
                Rol,
                Favoritos = new List<object>(),
                Prestamos = new List<object>(),
                Feedbacks = new List<object>(),
                Transacciones = new List<object>(),
                ListaNegra = new List<object>()
            };

            var response = await httpClient.PostAsJsonAsync(apiUrl, nuevoUsuario);

            if (response.IsSuccessStatusCode)
            {
                Exito = true;
                Mensaje = "Usuario registrado con éxito.";
                return RedirectToPage("/Login");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Exito = false;
                Mensaje = $"Error al registrar el usuario: {errorContent}";
                return Page();
            }
        }
    }
}
