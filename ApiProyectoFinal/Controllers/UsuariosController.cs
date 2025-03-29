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
    public class UsuariosController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public UsuariosController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioG5>>> GetUsuarios()
        {
            return await _context.FIDE_USUARIOSG5.ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioG5>> GetUsuario(int id)
        {
            var usuario = await _context.FIDE_USUARIOSG5.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return usuario;
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<UsuarioG5>> PostUsuario([FromBody] UsuarioG5 usuario)
        {
            _context.FIDE_USUARIOSG5.Add(usuario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.ID_Usuario }, usuario);
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] UsuarioG5 usuario)
        {
            if (id != usuario.ID_Usuario)
            {
                return BadRequest("El ID del usuario no coincide.");
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.FIDE_USUARIOSG5.Any(e => e.ID_Usuario == id))
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

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.FIDE_USUARIOSG5.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.FIDE_USUARIOSG5.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}