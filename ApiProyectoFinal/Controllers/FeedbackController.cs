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
    public class FeedbackController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public FeedbackController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: api/Feedback
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackG5>>> GetFeedbacks()
        {
            return await _context.FIDE_FEEDBACKG5
                .Include(f => f.Usuario) // Incluir datos del usuario
                .Include(f => f.Libro) // Incluir datos del libro
                .ToListAsync();
        }

        // GET: api/Feedback/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackG5>> GetFeedback(int id)
        {
            var feedback = await _context.FIDE_FEEDBACKG5
                .Include(f => f.Usuario) // Incluir datos del usuario
                .Include(f => f.Libro) // Incluir datos del libro
                .FirstOrDefaultAsync(f => f.ID_Feedback == id);

            if (feedback == null)
            {
                return NotFound();
            }
            return feedback;
        }

        // POST: api/Feedback
        [HttpPost]
        public async Task<ActionResult<FeedbackG5>> PostFeedback([FromBody] FeedbackG5 feedback)
        {
            _context.FIDE_FEEDBACKG5.Add(feedback);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFeedback), new { id = feedback.ID_Feedback }, feedback);
        }

        // PUT: api/Feedback/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedback(int id, [FromBody] FeedbackG5 feedback)
        {
            if (id != feedback.ID_Feedback)
            {
                return BadRequest("El ID del feedback no coincide.");
            }

            _context.Entry(feedback).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.FIDE_FEEDBACKG5.Any(e => e.ID_Feedback == id))
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

        // DELETE: api/Feedback/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var feedback = await _context.FIDE_FEEDBACKG5.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            _context.FIDE_FEEDBACKG5.Remove(feedback);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}