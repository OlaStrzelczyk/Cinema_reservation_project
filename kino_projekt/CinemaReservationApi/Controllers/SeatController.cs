using Microsoft.AspNetCore.Mvc;
using CinemaReservationApi.Data;
using CinemaReservationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SeatsController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Pobierz miejsca (opcjonalnie z filteringiem po screeningId)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seat>>> GetSeats([FromQuery] int? screeningId)
        {
            if (screeningId.HasValue)
            {
                return await _context.Seats
                    .Where(s => s.ScreeningId == screeningId)
                    .ToListAsync();
            }

            return await _context.Seats.ToListAsync();
        }

        // ➕ Dodaj jedno miejsce
        [HttpPost]
        public async Task<ActionResult<Seat>> Create(Seat seat)
        {
            _context.Seats.Add(seat);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSeats), new { id = seat.Id }, seat);
        }

        // 🛠️ Zaktualizuj miejsce
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Seat updatedSeat)
        {
            if (id != updatedSeat.Id)
            {
                return BadRequest("ID w URL i w przesłanym obiekcie muszą być zgodne.");
            }

            var seat = await _context.Seats.FindAsync(id);
            if (seat == null)
            {
                return NotFound("Nie znaleziono miejsca o podanym ID.");
            }

            // 🔒 Opcjonalnie sprawdź, czy miejsce jest zarezerwowane na jakimkolwiek seansie
            bool isSeatReserved = await _context.Reservations
                .AnyAsync(r => r.SeatId == seat.Id);

            if (isSeatReserved && seat.SeatNumber != updatedSeat.SeatNumber)
            {
                return BadRequest("Nie można zmienić numeru miejsca, jeśli już zostało zarezerwowane.");
            }

            seat.SeatNumber = updatedSeat.SeatNumber;
            seat.ScreeningId = updatedSeat.ScreeningId;

            await _context.SaveChangesAsync();

            return Ok(seat);
        }

        // ❌ Usuń miejsce
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var seat = await _context.Seats.FindAsync(id);
            if (seat == null)
            {
                return NotFound("Nie znaleziono miejsca o podanym ID.");
            }

            // 🔒 Nie pozwalaj usunąć, jeśli jakakolwiek rezerwacja istnieje
            bool isSeatReserved = await _context.Reservations
                .AnyAsync(r => r.SeatId == seat.Id);

            if (isSeatReserved)
            {
                return BadRequest("Nie można usunąć miejsca, które zostało już zarezerwowane.");
            }

            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();

            return Ok($"Miejsce o ID {id} zostało usunięte.");
        }

        // 📦 Dodaj wiele miejsc jednocześnie
        [HttpPost("bulk")]
        public async Task<IActionResult> CreateMany([FromBody] List<Seat> seats)
        {
            if (seats == null || seats.Count == 0)
            {
                return BadRequest("Lista miejsc nie może być pusta.");
            }

            _context.Seats.AddRange(seats);
            await _context.SaveChangesAsync();

            return Ok(seats);
        }
    }
}
