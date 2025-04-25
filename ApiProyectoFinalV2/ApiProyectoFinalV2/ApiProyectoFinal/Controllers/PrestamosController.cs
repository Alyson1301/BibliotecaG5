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
    public class PrestamosController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public PrestamosController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: api/Prestamos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrestamoG5>>> GetPrestamos()
        {
            return await _context.FIDE_PRESTAMOSG5
                .Include(p => p.Usuario) // Incluir datos del usuario
                .Include(p => p.Libro) // Incluir datos del libro
                .ToListAsync();
        }

        // GET: api/Prestamos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PrestamoG5>> GetPrestamo(int id)
        {
            var prestamo = await _context.FIDE_PRESTAMOSG5
                .Include(p => p.Usuario) // Incluir datos del usuario
                .Include(p => p.Libro) // Incluir datos del libro
                .FirstOrDefaultAsync(p => p.ID_Prestamo == id);

            if (prestamo == null)
            {
                return NotFound();
            }
            return prestamo;
        }

        // POST: api/Prestamos
        [HttpPost]
        public async Task<ActionResult<PrestamoG5>> PostPrestamo([FromBody] PrestamoG5 prestamo)
        {
            _context.FIDE_PRESTAMOSG5.Add(prestamo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPrestamo), new { id = prestamo.ID_Prestamo }, prestamo);
        }

        // PUT: api/Prestamos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrestamo(int id, [FromBody] PrestamoG5 prestamo)
        {
            if (id != prestamo.ID_Prestamo)
            {
                return BadRequest("El ID del préstamo no coincide.");
            }

            _context.Entry(prestamo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.FIDE_PRESTAMOSG5.Any(e => e.ID_Prestamo == id))
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

        // DELETE: api/Prestamos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrestamo(int id)
        {
            var prestamo = await _context.FIDE_PRESTAMOSG5.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }

            _context.FIDE_PRESTAMOSG5.Remove(prestamo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}