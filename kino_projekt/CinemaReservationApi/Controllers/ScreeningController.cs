using Microsoft.AspNetCore.Mvc;
using CinemaReservationApi.Data;
using CinemaReservationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreeningsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ScreeningsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Screenings?movieId=1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Screening>>> GetAll([FromQuery] int? movieId)
        {
            try
            {
                var query = _context.Screenings.AsQueryable();

                if (movieId.HasValue)
                {
                    query = query.Where(s => s.MovieId == movieId);
                }

                var result = await query.ToListAsync();

                // 🔒 WYŁĄCZENIE CACHE
                Response.Headers["Cache-Control"] = "no-store";

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("🔥 Błąd w GetAll: " + ex.Message);
                return StatusCode(500, "Wewnętrzny błąd serwera: " + ex.Message);
            }
        }

        // GET: api/Screenings/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Screening>> GetById(int id)
        {
            var screening = await _context.Screenings.FindAsync(id);

            if (screening == null)
            {
                return NotFound($"Nie znaleziono seansu o ID {id}.");
            }

            return Ok(screening);
        }

        // POST: api/Screenings
        [HttpPost]
        public async Task<ActionResult<Screening>> Create(Screening screening)
        {
            _context.Screenings.Add(screening);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = screening.Id }, screening);
        }

        // PUT: api/Screenings/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Screening updatedScreening)
        {
            if (id != updatedScreening.Id)
                return BadRequest("ID w URL i obiekcie muszą być zgodne.");

            var screening = await _context.Screenings.FindAsync(id);
            if (screening == null)
                return NotFound("Nie znaleziono seansu.");

            screening.MovieId = updatedScreening.MovieId;
            screening.ScreeningTime = updatedScreening.ScreeningTime;
            screening.HallNumber = updatedScreening.HallNumber;

            await _context.SaveChangesAsync();

            return Ok(screening);
        }

        // DELETE: api/Screenings/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var screening = await _context.Screenings
                .Include(s => s.Reservations)
                .Include(s => s.Seats)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (screening == null)
                return NotFound("Nie znaleziono seansu o podanym ID.");

            _context.Reservations.RemoveRange(screening.Reservations);
            _context.Seats.RemoveRange(screening.Seats);
            _context.Screenings.Remove(screening);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Screenings/bulk
        [HttpPost("bulk")]
        [ProducesResponseType(typeof(List<Screening>), 200)]
        public async Task<IActionResult> CreateMany([FromBody] List<Screening> screenings)
        {
            if (screenings == null || !screenings.Any())
                return BadRequest("Lista seansów nie może być pusta.");

            _context.Screenings.AddRange(screenings);
            await _context.SaveChangesAsync();

            return Ok(screenings);
        }
    }
}
