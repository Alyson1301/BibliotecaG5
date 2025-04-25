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
    public class DisponibilidadController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public DisponibilidadController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: api/Disponibilidad
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisponibilidadG5>>> GetDisponibilidades()
        {
            return await _context.FIDE_DISPONIBILIDADG5.ToListAsync(); // Eliminado .Include(d => d.Libro)
        }

        // GET: api/Disponibilidad/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DisponibilidadG5>> GetDisponibilidad(int id)
        {
            var disponibilidad = await _context.FIDE_DISPONIBILIDADG5.FindAsync(id); // Eliminado .Include(d => d.Libro)

            if (disponibilidad == null)
            {
                return NotFound();
            }
            return disponibilidad;
        }


        // POST: api/Disponibilidad
        [HttpPost]
        public async Task<ActionResult<DisponibilidadG5>> PostDisponibilidad([FromBody] DisponibilidadG5 disponibilidad)
        {
            _context.FIDE_DISPONIBILIDADG5.Add(disponibilidad);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDisponibilidad), new { id = disponibilidad.ID_Disponibilidad }, disponibilidad);
        }

        // PUT: api/Disponibilidad/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDisponibilidad(int id, [FromBody] DisponibilidadG5 disponibilidad)
        {
            if (id != disponibilidad.ID_Disponibilidad)
            {
                return BadRequest("El ID de la disponibilidad no coincide.");
            }

            _context.Entry(disponibilidad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.FIDE_DISPONIBILIDADG5.Any(e => e.ID_Disponibilidad == id))
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

        // DELETE: api/Disponibilidad/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisponibilidad(int id)
        {
            var disponibilidad = await _context.FIDE_DISPONIBILIDADG5.FindAsync(id);
            if (disponibilidad == null)
            {
                return NotFound();
            }

            _context.FIDE_DISPONIBILIDADG5.Remove(disponibilidad);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
