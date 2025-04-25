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
    public class AutoresController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public AutoresController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: api/Autores 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutorG5>>> GetAutores()
        {
            return await _context.FIDE_AUTORESG5
                .Include(a => a.Libros)
                .ToListAsync();
        }

        // GET: api/Autores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AutorG5>> GetAutor(int id)
        {
            var autor = await _context.FIDE_AUTORESG5
                .Include(a => a.Libros)
                .FirstOrDefaultAsync(a => a.ID_Autor == id);

            if (autor == null)
            {
                return NotFound();
            }

            return autor;
        }

        // POST: api/Autores
        [HttpPost]
        public async Task<ActionResult<AutorG5>> PostAutor([FromBody] AutorG5 autor)
        {
            _context.FIDE_AUTORESG5.Add(autor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAutor), new { id = autor.ID_Autor }, autor);
        }

        // PUT: api/Autores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAutor(int id, [FromBody] AutorG5 autor)
        {
            if (id != autor.ID_Autor)
            {
                return BadRequest("El ID del autor no coincide.");
            }

            // Obtener el autor existente para preservar las relaciones
            var autorExistente = await _context.FIDE_AUTORESG5
                .Include(a => a.Libros)
                .FirstOrDefaultAsync(a => a.ID_Autor == id);

            if (autorExistente == null)
            {
                return NotFound();
            }

            // Actualizar solo los campos enviados en la solicitud
            autorExistente.Nombre = autor.Nombre;
            autorExistente.Nacionalidad = autor.Nacionalidad;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.FIDE_AUTORESG5.Any(e => e.ID_Autor == id))
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

        // DELETE: api/Autores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAutor(int id)
        {
            var autor = await _context.FIDE_AUTORESG5.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }

            _context.FIDE_AUTORESG5.Remove(autor);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}