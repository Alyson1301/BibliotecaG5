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
    public class TransaccionesController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public TransaccionesController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: api/Transacciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransaccionG5>>> GetTransacciones()
        {
            return await _context.FIDE_TRANSACCIONESG5
                .Include(t => t.Usuario) // Incluir datos del usuario
                .Include(t => t.Libro) // Incluir datos del libro
                .ToListAsync();
        }

        // GET: api/Transacciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransaccionG5>> GetTransaccion(int id)
        {
            var transaccion = await _context.FIDE_TRANSACCIONESG5
                .Include(t => t.Usuario) // Incluir datos del usuario
                .Include(t => t.Libro) // Incluir datos del libro
                .FirstOrDefaultAsync(t => t.ID_Transaccion == id);

            if (transaccion == null)
            {
                return NotFound();
            }
            return transaccion;
        }

        // POST: api/Transacciones
        [HttpPost]
        public async Task<ActionResult<TransaccionG5>> PostTransaccion([FromBody] TransaccionG5 transaccion)
        {
            _context.FIDE_TRANSACCIONESG5.Add(transaccion);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTransaccion), new { id = transaccion.ID_Transaccion }, transaccion);
        }

        // PUT: api/Transacciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaccion(int id, [FromBody] TransaccionG5 transaccion)
        {
            if (id != transaccion.ID_Transaccion)
            {
                return BadRequest("El ID de la transacción no coincide.");
            }

            _context.Entry(transaccion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.FIDE_TRANSACCIONESG5.Any(e => e.ID_Transaccion == id))
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

        // DELETE: api/Transacciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaccion(int id)
        {
            var transaccion = await _context.FIDE_TRANSACCIONESG5.FindAsync(id);
            if (transaccion == null)
            {
                return NotFound();
            }

            _context.FIDE_TRANSACCIONESG5.Remove(transaccion);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}