using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace PrograAvanBiblioteca.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public string Correo { get; set; } = string.Empty;

        [BindProperty]
        public string Contraseña { get; set; } = string.Empty;

        public string Mensaje { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {

            var client = _httpClientFactory.CreateClient();

            var loginRequest = new LoginRequest
            {
                correo = Correo,
                contraseña = Contraseña
            };

            var response = await client.PostAsJsonAsync("https://localhost:7058/api/Usuarios/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                if (result != null && result.isValid)
                {
                   
                    HttpContext.Session.SetString("UsuarioId", result.Id.ToString()); 

                    HttpContext.Session.SetString("Nombre", result.Nombre);
                    HttpContext.Session.SetString("Apellido", result.Apellido);
                    HttpContext.Session.SetString("Correo", result.Correo);
                    HttpContext.Session.SetString("Rol", result.Rol);

                    return RedirectToPage("/Index");
                }

                Mensaje = "Error al procesar la respuesta del servidor.";
                return Page();
            }

            Mensaje = "Credenciales incorrectas.";
            return Page();
        }

        public class LoginRequest
        {
            public string correo { get; set; } = string.Empty;
            public string contraseña { get; set; } = string.Empty;
        }

        public class LoginResponse
        {
            [JsonPropertyName("isValid")]
            public bool isValid { get; set; }

            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("nombre")]
            public string Nombre { get; set; } = string.Empty;

            [JsonPropertyName("apellido")]
            public string Apellido { get; set; } = string.Empty;

            [JsonPropertyName("correo")]
            public string Correo { get; set; } = string.Empty;

            [JsonPropertyName("rol")]
            public string Rol { get; set; } = string.Empty;
        }
    }
}
