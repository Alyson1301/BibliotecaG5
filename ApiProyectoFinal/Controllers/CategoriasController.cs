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
    public class CategoriasController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public CategoriasController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: api/Categorias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaG5>>> GetCategorias()
        {
            return await _context.FIDE_CATEGORIASG5.ToListAsync();
        }

        // GET: api/Categorias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaG5>> GetCategoria(int id)
        {
            var categoria = await _context.FIDE_CATEGORIASG5.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return categoria;
        }

        // POST: api/Categorias
        [HttpPost]
        public async Task<ActionResult<CategoriaG5>> PostCategoria([FromBody] CategoriaG5 categoria)
        {
            _context.FIDE_CATEGORIASG5.Add(categoria);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.ID_Categoria }, categoria);
        }

        // PUT: api/Categorias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, [FromBody] CategoriaG5 categoria)
        {
            if (id != categoria.ID_Categoria)
            {
                return BadRequest("El ID de la categoría no coincide.");
            }

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.FIDE_CATEGORIASG5.Any(e => e.ID_Categoria == id))
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

        // DELETE: api/Categorias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.FIDE_CATEGORIASG5.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.FIDE_CATEGORIASG5.Remove(categoria);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}