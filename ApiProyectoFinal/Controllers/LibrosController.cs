using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiProyectoFinal.Data;
using ApiProyectoFinal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            return await _context.FIDE_LIBROSG5
                .Include(l => l.Autor) // Incluir datos del autor
                .Include(l => l.Categoria) // Incluir datos de la categoría
                .ToListAsync();
        }

        // GET: api/Libros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LibroG5>> GetLibro(int id)
        {
            var libro = await _context.FIDE_LIBROSG5
                .Include(l => l.Autor) // Incluir datos del autor
                .Include(l => l.Categoria) // Incluir datos de la categoría
                .FirstOrDefaultAsync(l => l.ID_Libro == id);

            if (libro == null)
            {
                return NotFound();
            }
            return libro;
        }

        // POST: api/Libros
        [HttpPost]
        public async Task<ActionResult<LibroG5>> PostLibro([FromBody] LibroG5 libro)
        {
            _context.FIDE_LIBROSG5.Add(libro);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLibro), new { id = libro.ID_Libro }, libro);
        }

        // PUT: api/Libros/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibro(int id, [FromBody] LibroG5 libro)
        {
            if (id != libro.ID_Libro)
            {
                return BadRequest("El ID del libro no coincide.");
            }

            _context.Entry(libro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.FIDE_LIBROSG5.Any(e => e.ID_Libro == id))
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

            _context.FIDE_LIBROSG5.Remove(libro);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}