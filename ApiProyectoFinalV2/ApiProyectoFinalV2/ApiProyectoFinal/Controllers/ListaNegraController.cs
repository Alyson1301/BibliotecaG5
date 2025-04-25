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
    public class ListaNegraController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public ListaNegraController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: api/ListaNegra
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListaNegraG5>>> GetListaNegra()
        {
            return await _context.FIDE_LISTA_NEGRAG5
                .Include(ln => ln.Usuario) 
                .ToListAsync();
        }

        // GET: api/ListaNegra/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ListaNegraG5>> GetListaNegra(int id)
        {
            var listaNegra = await _context.FIDE_LISTA_NEGRAG5
                .Include(ln => ln.Usuario) 
                .FirstOrDefaultAsync(ln => ln.ID_Lista_Negra == id);

            if (listaNegra == null)
            {
                return NotFound();
            }
            return listaNegra;
        }

        // POST: api/ListaNegra
        [HttpPost]
        public async Task<ActionResult<ListaNegraG5>> PostListaNegra([FromBody] ListaNegraG5 listaNegra)
        {
            _context.FIDE_LISTA_NEGRAG5.Add(listaNegra);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetListaNegra), new { id = listaNegra.ID_Lista_Negra }, listaNegra);
        }

        // PUT: api/ListaNegra/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutListaNegra(int id, [FromBody] ListaNegraG5 listaNegra)
        {
            if (id != listaNegra.ID_Lista_Negra)
            {
                return BadRequest("El ID de la lista negra no coincide.");
            }

            _context.Entry(listaNegra).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.FIDE_LISTA_NEGRAG5.Any(e => e.ID_Lista_Negra == id))
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

        // DELETE: api/ListaNegra/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteListaNegra(int id)
        {
            var listaNegra = await _context.FIDE_LISTA_NEGRAG5.FindAsync(id);
            if (listaNegra == null)
            {
                return NotFound();
            }

            _context.FIDE_LISTA_NEGRAG5.Remove(listaNegra);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}