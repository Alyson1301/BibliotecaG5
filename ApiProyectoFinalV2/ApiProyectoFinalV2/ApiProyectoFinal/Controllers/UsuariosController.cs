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
    public class UsuariosController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public UsuariosController(BibliotecaContext context)
        {
            _context = context;
        }

        // MODELO TEMPORAL PARA LOGIN
        public class LoginRequest
        {
            public string Correo { get; set; }
            public string Contraseña { get; set; }
        }

        // MODELO PARA RESETEAR PASSWORD
        public class ResetPasswordRequest
        {
            public string Correo { get; set; }
            public string NuevaContraseña { get; set; }
            public string Token { get; set; }
        }

        // POST: api/Usuarios/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // DEBUG: imprimir los datos entrantes
            Console.WriteLine($"Login recibido: {request.Correo} - {request.Contraseña}");

            var usuario = await _context.FIDE_USUARIOSG5
                .FirstOrDefaultAsync(u => u.Correo == request.Correo && u.Contraseña == request.Contraseña);

            if (usuario == null)
            {
                return Unauthorized(new { isValid = false, mensaje = "Credenciales inválidas" });
            }

            return Ok(new
            {
                isValid = true,
                Id = usuario.ID_Usuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo = usuario.Correo,
                Rol = usuario.Rol
            });
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

        // POST: api/Usuarios (registro)
        [HttpPost]
        public async Task<IActionResult> RegistrarUsuario([FromBody] UsuarioG5 nuevoUsuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool correoExiste = await _context.FIDE_USUARIOSG5
                .AnyAsync(u => u.Correo == nuevoUsuario.Correo);

            if (correoExiste)
            {
                return Conflict(new { mensaje = "El correo ya está registrado." });
            }

            _context.FIDE_USUARIOSG5.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = nuevoUsuario.ID_Usuario }, nuevoUsuario);
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

        // PUT: api/Usuarios/resetearPassword
        [HttpPut("resetearPassword")]
        public async Task<IActionResult> ResetearPassword([FromBody] ResetPasswordRequest request)
        {
            
            Console.WriteLine($"API: ResetearPassword - Correo: {request.Correo}, Token: {request.Token}, NuevaContraseña presente: {!string.IsNullOrEmpty(request.NuevaContraseña)}");

            if (string.IsNullOrEmpty(request.Correo) || string.IsNullOrEmpty(request.NuevaContraseña))
            {
                return BadRequest("Correo y nueva contraseña son obligatorios");
            }

            var usuario = await _context.FIDE_USUARIOSG5.FirstOrDefaultAsync(u => u.Correo == request.Correo);

            if (usuario == null)
                return NotFound("Usuario no encontrado.");

           

            usuario.Contraseña = request.NuevaContraseña;
            await _context.SaveChangesAsync();

            return Ok("Contraseña actualizada correctamente.");
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