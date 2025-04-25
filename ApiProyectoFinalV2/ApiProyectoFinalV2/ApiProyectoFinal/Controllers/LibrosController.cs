using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiProyectoFinal.Data;
using ApiProyectoFinal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ApiProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public LibrosController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: api/Libros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroG5>>> GetLibros()
        {
            var libros = await _context.FIDE_LIBROSG5
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .ToListAsync();

           
            foreach (var libro in libros)
            {
                var disponibilidad = await _context.FIDE_DISPONIBILIDADG5
                    .Where(d => d.ID_Libro == libro.ID_Libro)
                    .OrderByDescending(d => d.ID_Disponibilidad)
                    .FirstOrDefaultAsync();

                libro.EstadoDisponibilidad = disponibilidad?.Estado ?? "Disponible";
            }

            return libros;
        }

        // GET: api/Libros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LibroG5>> GetLibro(int id)
        {
            var libro = await _context.FIDE_LIBROSG5
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .FirstOrDefaultAsync(l => l.ID_Libro == id);

            if (libro == null)
            {
                return NotFound();
            }

            
            var disponibilidad = await _context.FIDE_DISPONIBILIDADG5
                .Where(d => d.ID_Libro == libro.ID_Libro)
                .OrderByDescending(d => d.ID_Disponibilidad)
                .FirstOrDefaultAsync();

            libro.EstadoDisponibilidad = disponibilidad?.Estado ?? "Disponible";

            return libro;
        }

        // POST: api/Libros
        [HttpPost]
        public async Task<ActionResult<LibroG5>> PostLibro([FromBody] LibroInsertDTO libroDTO)
        {
            
            var autor = await _context.FIDE_AUTORESG5.FindAsync(libroDTO.ID_Autor);
            if (autor == null)
            {
                return BadRequest("El autor especificado no existe");
            }

           
            if (libroDTO.ID_Categoria.HasValue)
            {
                var categoria = await _context.FIDE_CATEGORIASG5.FindAsync(libroDTO.ID_Categoria.Value);
                if (categoria == null)
                {
                    return BadRequest("La categoría especificada no existe");
                }
            }

            var libro = new LibroG5
            {
                Titulo = libroDTO.Titulo,
                ID_Autor = libroDTO.ID_Autor,
                ID_Categoria = libroDTO.ID_Categoria,
                ISBN = libroDTO.ISBN,
                Año_Publicación = libroDTO.Año_Publicación
            };

            _context.FIDE_LIBROSG5.Add(libro);
            await _context.SaveChangesAsync();

            
            var disponibilidad = new DisponibilidadG5
            {
                ID_Libro = libro.ID_Libro,
                Estado = libroDTO.EstadoDisponibilidad
            };

            _context.FIDE_DISPONIBILIDADG5.Add(disponibilidad);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLibro), new { id = libro.ID_Libro }, libro);
        }

        // PUT: api/Libros/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibro(int id, [FromBody] LibroUpdateDTO libroDTO)
        {
            if (id != libroDTO.ID_Libro)
            {
                return BadRequest("El ID del libro no coincide.");
            }

      
            var autor = await _context.FIDE_AUTORESG5.FindAsync(libroDTO.ID_Autor);
            if (autor == null)
            {
                return BadRequest("El autor especificado no existe");
            }

            
            if (libroDTO.ID_Categoria.HasValue)
            {
                var categoria = await _context.FIDE_CATEGORIASG5.FindAsync(libroDTO.ID_Categoria.Value);
                if (categoria == null)
                {
                    return BadRequest("La categoría especificada no existe");
                }
            }

            
            var libroExistente = await _context.FIDE_LIBROSG5.FindAsync(id);
            if (libroExistente == null)
            {
                return NotFound();
            }

            
            libroExistente.Titulo = libroDTO.Titulo;
            libroExistente.ID_Autor = libroDTO.ID_Autor;
            libroExistente.ID_Categoria = libroDTO.ID_Categoria;
            libroExistente.ISBN = libroDTO.ISBN;
            libroExistente.Año_Publicación = libroDTO.Año_Publicación;

            
            _context.Entry(libroExistente).Property(x => x.Titulo).IsModified = true;
            _context.Entry(libroExistente).Property(x => x.ID_Autor).IsModified = true;
            _context.Entry(libroExistente).Property(x => x.ID_Categoria).IsModified = true;
            _context.Entry(libroExistente).Property(x => x.ISBN).IsModified = true;
            _context.Entry(libroExistente).Property(x => x.Año_Publicación).IsModified = true;

            try
            {
                await _context.SaveChangesAsync();

                
                var ultimaDisponibilidad = await _context.FIDE_DISPONIBILIDADG5
                    .Where(d => d.ID_Libro == id)
                    .OrderByDescending(d => d.ID_Disponibilidad)
                    .FirstOrDefaultAsync();

                if (ultimaDisponibilidad == null || ultimaDisponibilidad.Estado != libroDTO.EstadoDisponibilidad)
                {
                    var nuevaDisponibilidad = new DisponibilidadG5
                    {
                        ID_Libro = id,
                        Estado = libroDTO.EstadoDisponibilidad
                    };

                    _context.FIDE_DISPONIBILIDADG5.Add(nuevaDisponibilidad);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Libros/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibro(int id)
        {
            var libro = await _context.FIDE_LIBROSG5.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }

         
            var disponibilidades = await _context.FIDE_DISPONIBILIDADG5
                .Where(d => d.ID_Libro == id)
                .ToListAsync();

            _context.FIDE_DISPONIBILIDADG5.RemoveRange(disponibilidades);
            _context.FIDE_LIBROSG5.Remove(libro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Libros/buscar/titulo?q=valor
        [HttpGet("buscar/titulo")]
        public async Task<ActionResult<IEnumerable<LibroG5>>> BuscarPorTitulo(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return await GetLibros();
            }

            var libros = await _context.FIDE_LIBROSG5
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .Where(l => l.Titulo.Contains(q))
                .ToListAsync();

           
            foreach (var libro in libros)
            {
                var disponibilidad = await _context.FIDE_DISPONIBILIDADG5
                    .Where(d => d.ID_Libro == libro.ID_Libro)
                    .OrderByDescending(d => d.ID_Disponibilidad)
                    .FirstOrDefaultAsync();

                libro.EstadoDisponibilidad = disponibilidad?.Estado ?? "Disponible";
            }

            return libros;
        }

        // GET: api/Libros/categoria/1
        [HttpGet("categoria/{id}")]
        public async Task<ActionResult<IEnumerable<LibroG5>>> BuscarPorCategoria(int id)
        {
            var libros = await _context.FIDE_LIBROSG5
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .Where(l => l.ID_Categoria == id)
                .ToListAsync();

            
            foreach (var libro in libros)
            {
                var disponibilidad = await _context.FIDE_DISPONIBILIDADG5
                    .Where(d => d.ID_Libro == libro.ID_Libro)
                    .OrderByDescending(d => d.ID_Disponibilidad)
                    .FirstOrDefaultAsync();

                libro.EstadoDisponibilidad = disponibilidad?.Estado ?? "Disponible";
            }

            return libros;
        }

        private bool LibroExists(int id)
        {
            return _context.FIDE_LIBROSG5.Any(e => e.ID_Libro == id);
        }
    }
}