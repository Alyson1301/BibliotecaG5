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
    public class FavoritosController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public FavoritosController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: api/Favoritos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FavoritoG5>>> GetFavoritos()
        {
            return await _context.FIDE_FAVORITOSG5
                .Include(f => f.Usuario) // Incluir datos del usuario
                .Include(f => f.Libro) // Incluir datos del libro
                .ToListAsync();
        }

        // GET: api/Favoritos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FavoritoG5>> GetFavorito(int id)
        {
            var favorito = await _context.FIDE_FAVORITOSG5
                .Include(f => f.Usuario) // Incluir datos del usuario
                .Include(f => f.Libro) // Incluir datos del libro
                .FirstOrDefaultAsync(f => f.ID_Favorito == id);

            if (favorito == null)
            {
                return NotFound();
            }
            return favorito;
        }

        // POST: api/Favoritos
        [HttpPost]
        public async Task<ActionResult<FavoritoG5>> PostFavorito([FromBody] FavoritoG5 favorito)
        {
            _context.FIDE_FAVORITOSG5.Add(favorito);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFavorito), new { id = favorito.ID_Favorito }, favorito);
        }

        // PUT: api/Favoritos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFavorito(int id, [FromBody] FavoritoG5 favorito)
        {
            if (id != favorito.ID_Favorito)
            {
                return BadRequest("El ID del favorito no coincide.");
            }

            _context.Entry(favorito).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.FIDE_FAVORITOSG5.Any(e => e.ID_Favorito == id))
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

        // DELETE: api/Favoritos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFavorito(int id)
        {
            var favorito = await _context.FIDE_FAVORITOSG5.FindAsync(id);
            if (favorito == null)
            {
                return NotFound();
            }

            _context.FIDE_FAVORITOSG5.Remove(favorito);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}