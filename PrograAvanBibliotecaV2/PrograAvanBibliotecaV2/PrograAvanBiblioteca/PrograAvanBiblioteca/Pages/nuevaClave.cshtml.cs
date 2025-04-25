using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PrograAvanBiblioteca.Pages
{
    public class NuevaClaveModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NuevaClaveModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public string Token { get; set; }

        [BindProperty(SupportsGet = true, Name = "email")]
        public string Correo { get; set; }

        [BindProperty]
        public string NuevaClave { get; set; }

        public string Mensaje { get; set; }
        public bool Exito { get; set; }

        public void OnGet()
        {
            if (string.IsNullOrEmpty(Token) || string.IsNullOrEmpty(Correo))
            {
                Mensaje = "El enlace de recuperación está incompleto. Por favor revisa tu correo y utiliza el enlace completo proporcionado.";
                Exito = false;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(NuevaClave))
            {
                Mensaje = "Por favor ingresa la nueva contraseña.";
                Exito = false;
                return Page();
            }

            var cliente = _httpClientFactory.CreateClient();
            var url = "https://localhost:7058/api/Usuarios/resetearPassword";

            var data = new
            {
                Correo = Correo,
                Token = Token,
                NuevaContraseña = NuevaClave
            };

            var respuesta = await cliente.PutAsJsonAsync(url, data);

            if (respuesta.IsSuccessStatusCode)
            {
                Exito = true;
                Mensaje = "La contraseña ha sido restablecida correctamente.";
                return RedirectToPage("/Login");
            }
            else
            {
                Exito = false;
                Mensaje = "Error al restablecer la contraseña. Verifica que el enlace sea válido.";
                return Page();
            }
        }
    }
}
