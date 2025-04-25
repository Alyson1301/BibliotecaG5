using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PrograAvanBiblioteca.Pages
{
    public class menuAutoresModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiUrl = "https://localhost:7058/api/Autores";

        public menuAutoresModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<AutorG5> Autores { get; set; } = new List<AutorG5>();
        public string Mensaje { get; set; } = string.Empty;

        [BindProperty]
        public int ID_Autor { get; set; }

        [BindProperty]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        public string Nacionalidad { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                // Forzar la obtención de datos sin caché
                var request = new HttpRequestMessage(HttpMethod.Get, _apiUrl);
                request.Headers.Add("Cache-Control", "no-cache");
                request.Headers.Add("Pragma", "no-cache");
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    Autores = await response.Content.ReadFromJsonAsync<List<AutorG5>>() ?? new List<AutorG5>();
                }
                else
                {
                    Mensaje = $"Error al obtener los datos de autores. Código: {response.StatusCode}";
                    Console.WriteLine($"Error API: {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                Mensaje = $"Error inesperado: {ex.Message}";
                Console.WriteLine($"Excepción: {ex}");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Nacionalidad))
            {
                Mensaje = "El nombre y la nacionalidad son obligatorios.";
                return await OnGetAsync();
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var nuevoAutor = new AutorG5
                {
                    Nombre = Nombre,
                    Nacionalidad = Nacionalidad,
                    Libros = new List<LibroG5>()
                };

                var response = await client.PostAsJsonAsync(_apiUrl, nuevoAutor);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/menuAutores");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Mensaje = $"Error al registrar el autor: {errorContent}";
                    Console.WriteLine($"Error API (POST): {errorContent}");
                    return await OnGetAsync();
                }
            }
            catch (Exception ex)
            {
                Mensaje = $"Error al registrar: {ex.Message}";
                Console.WriteLine($"Excepción en POST: {ex}");
                return await OnGetAsync();
            }
        }

        public async Task<IActionResult> OnPostEditarAutorAsync()
        {
            if (ID_Autor == 0 || string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Nacionalidad))
            {
                Mensaje = "El ID, nombre y nacionalidad son obligatorios para editar.";
                return await OnGetAsync();
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var autorEditado = new AutorG5
                {
                    ID_Autor = ID_Autor,
                    Nombre = Nombre,
                    Nacionalidad = Nacionalidad
                };

                Console.WriteLine($"Enviando PUT a {_apiUrl}/{ID_Autor}");
                var response = await client.PutAsJsonAsync($"{_apiUrl}/{ID_Autor}", autorEditado);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/menuAutores");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Mensaje = $"Error al editar el autor: {errorContent} - Código: {response.StatusCode}";
                    Console.WriteLine($"Error API (PUT): {errorContent}");
                    return await OnGetAsync();
                }
            }
            catch (Exception ex)
            {
                Mensaje = $"Error al editar: {ex.Message}";
                Console.WriteLine($"Excepción en PUT: {ex}");
                return await OnGetAsync();
            }
        }

        public async Task<IActionResult> OnPostEliminarAutorAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var apiUrl = $"{_apiUrl}/{id}";
                var response = await client.DeleteAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/menuAutores");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Mensaje = $"Error al eliminar el autor: {errorContent}";
                    Console.WriteLine($"Error API (DELETE): {errorContent}");
                    return await OnGetAsync();
                }
            }
            catch (Exception ex)
            {
                Mensaje = $"Error al eliminar: {ex.Message}";
                Console.WriteLine($"Excepción en DELETE: {ex}");
                return await OnGetAsync();
            }
        }

        public class AutorG5
        {
            [JsonPropertyName("id_Autor")]
            public int ID_Autor { get; set; }

            [JsonPropertyName("nombre")]
            public string Nombre { get; set; } = string.Empty;

            [JsonPropertyName("nacionalidad")]
            public string Nacionalidad { get; set; } = string.Empty;

            [JsonPropertyName("libros")]
            public List<LibroG5> Libros { get; set; } = new List<LibroG5>();
        }

        public class LibroG5
        {
            [JsonPropertyName("titulo")]
            public string Titulo { get; set; } = string.Empty;
        }
    }
}