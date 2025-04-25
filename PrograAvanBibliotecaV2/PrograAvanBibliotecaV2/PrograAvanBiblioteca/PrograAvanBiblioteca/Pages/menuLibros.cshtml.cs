using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace PrograAvanBiblioteca.Pages
{
    public class menuLibrosModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:7058/api";

        public menuLibrosModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [BindProperty]
        public int ID_Libro { get; set; }

        [BindProperty]
        public string Titulo { get; set; }

        [BindProperty]
        public int ID_Autor { get; set; }

        [BindProperty]
        public int? ID_Categoria { get; set; }

        [BindProperty]
        public string ISBN { get; set; }

        [BindProperty]
        public int Año_Publicación { get; set; }

        [BindProperty]
        public string EstadoDisponibilidad { get; set; } = "Disponible";

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoriaId { get; set; }

        public List<Libro> Libros { get; set; }
        public List<Autor> Autores { get; set; }
        public List<Categoria> Categorias { get; set; }
        public string Mensaje { get; set; }

        public async Task OnGetAsync()
        {
            await CargarListasAsync();

            // Aplicar filtros si existen
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Libros = Libros.Where(l =>
                    l.Titulo.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    l.ID_Libro.ToString() == SearchTerm ||
                    Autores.FirstOrDefault(a => a.ID_Autor == l.ID_Autor)?.Nombre.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) == true
                ).ToList();
            }
            else if (CategoriaId.HasValue)
            {
                Libros = Libros.Where(l => l.ID_Categoria == CategoriaId).ToList();
            }
        }

        public async Task<IActionResult> OnGetBuscarCategoriaAsync(int categoriaId)
        {
            CategoriaId = categoriaId;
            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostInsertarLibroAsync()
        {
            var libroNuevo = new LibroDto
            {
                Titulo = Titulo,
                ID_Autor = ID_Autor,
                ID_Categoria = ID_Categoria,
                ISBN = ISBN,
                Año_Publicación = Año_Publicación,
                EstadoDisponibilidad = EstadoDisponibilidad
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}/Libros", libroNuevo);

                if (response.IsSuccessStatusCode)
                {
                    Mensaje = "Libro insertado correctamente.";
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Mensaje = $"Error al insertar el libro: {error}";
                }
            }
            catch (Exception ex)
            {
                Mensaje = $"Error inesperado: {ex.Message}";
            }

            await CargarListasAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostEditarLibroAsync()
        {
            // Crear el DTO para la actualización
            var libroUpdateDto = new LibroUpdateDto
            {
                ID_Libro = ID_Libro,
                Titulo = Titulo,
                ID_Autor = ID_Autor,
                ID_Categoria = ID_Categoria,
                ISBN = ISBN,
                Año_Publicación = Año_Publicación,
                EstadoDisponibilidad = EstadoDisponibilidad
            };

            try
            {
                // Serializar manualmente para asegurar que se envían los datos correctos
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(libroUpdateDto, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_apiBaseUrl}/Libros/{ID_Libro}", content);

                if (response.IsSuccessStatusCode)
                {
                    Mensaje = "Libro actualizado correctamente.";
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Mensaje = $"Error al actualizar el libro: {error}";
                }
            }
            catch (Exception ex)
            {
                Mensaje = $"Error inesperado: {ex.Message}";
            }

            await CargarListasAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostEliminarLibroAsync()
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/Libros/{ID_Libro}");

                if (response.IsSuccessStatusCode)
                {
                    Mensaje = "Libro eliminado correctamente.";
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Mensaje = $"Error al eliminar el libro: {error}";
                }
            }
            catch (Exception ex)
            {
                Mensaje = $"Error inesperado: {ex.Message}";
            }

            await CargarListasAsync();
            return Page();
        }

        private async Task CargarListasAsync()
        {
            try
            {
                Libros = await _httpClient.GetFromJsonAsync<List<Libro>>($"{_apiBaseUrl}/Libros");
                Autores = await _httpClient.GetFromJsonAsync<List<Autor>>($"{_apiBaseUrl}/Autores");
                Categorias = await _httpClient.GetFromJsonAsync<List<Categoria>>($"{_apiBaseUrl}/Categorias");
            }
            catch (Exception ex)
            {
                Mensaje = $"Error al cargar datos: {ex.Message}";
                Libros = new List<Libro>();
                Autores = new List<Autor>();
                Categorias = new List<Categoria>();
            }
        }

        public class Libro
        {
            public int ID_Libro { get; set; }
            public string Titulo { get; set; }
            public int ID_Autor { get; set; }
            public int? ID_Categoria { get; set; }
            public string ISBN { get; set; }
            public int Año_Publicación { get; set; }
            public string EstadoDisponibilidad { get; set; }
        }

        public class LibroDto
        {
            public string Titulo { get; set; }
            public int ID_Autor { get; set; }
            public int? ID_Categoria { get; set; }
            public string ISBN { get; set; }
            public int Año_Publicación { get; set; }
            public string EstadoDisponibilidad { get; set; }
        }

        public class LibroUpdateDto
        {
            public int ID_Libro { get; set; }
            public string Titulo { get; set; }
            public int ID_Autor { get; set; }
            public int? ID_Categoria { get; set; }
            public string ISBN { get; set; }
            public int Año_Publicación { get; set; }
            public string EstadoDisponibilidad { get; set; }
        }

        public class Autor
        {
            public int ID_Autor { get; set; }
            public string Nombre { get; set; }
            public string Nacionalidad { get; set; }
        }

        public class Categoria
        {
            public int ID_Categoria { get; set; }
            public string Nombre { get; set; }
        }
    }
}