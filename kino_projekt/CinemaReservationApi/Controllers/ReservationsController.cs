using CinemaReservationApi.Data;
using CinemaReservationApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReservationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetAll()
        {
            var reservations = await _context.Reservations
                .Include(r => r.Seat)
                .Include(r => r.Screening)
                    .ThenInclude(s => s.Movie)
                .ToListAsync();

            return Ok(reservations);
        }

        // GET: api/Reservations/occupied?screeningId=1
        [HttpGet("occupied")]
        public async Task<IActionResult> GetOccupiedSeats(int screeningId)
        {
            var occupiedSeatIds = await _context.Reservations
                .Where(r => r.ScreeningId == screeningId)
                .Select(r => r.SeatId)
                .ToListAsync();

            return Ok(occupiedSeatIds);
        }

        // POST: api/Reservations
        [HttpPost]
        public async Task<IActionResult> PostReservation([FromBody] CreateReservationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var seat = await _context.Seats.FindAsync(dto.SeatId);
            var screening = await _context.Screenings.FindAsync(dto.ScreeningId);

            if (seat == null)
                return NotFound($"Seat with ID {dto.SeatId} not found.");
            if (screening == null)
                return NotFound($"Screening with ID {dto.ScreeningId} not found.");

            // 🔒 Sprawdzenie, czy to miejsce jest już zajęte na ten seans
            bool isAlreadyReserved = await _context.Reservations
                .AnyAsync(r => r.SeatId == dto.SeatId && r.ScreeningId == dto.ScreeningId);

            if (isAlreadyReserved)
                return BadRequest("This seat is already reserved for the selected screening.");

            var reservation = new Reservation
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                SeatId = dto.SeatId,
                ScreeningId = dto.ScreeningId
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return Ok(reservation);
        }

        // PUT: api/Reservations/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation(int id, [FromBody] CreateReservationDto dto)
        {
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null)
                return NotFound($"Reservation with ID {id} not found.");

            var seat = await _context.Seats.FindAsync(dto.SeatId);
            var screening = await _context.Screenings.FindAsync(dto.ScreeningId);

            if (seat == null)
                return NotFound($"Seat with ID {dto.SeatId} not found.");
            if (screening == null)
                return NotFound($"Screening with ID {dto.ScreeningId} not found.");

            // 🔒 Sprawdzenie, czy nowy wybór miejsca już nie jest zajęty przez inną rezerwację
            bool isSeatTaken = await _context.Reservations
                .AnyAsync(r => r.SeatId == dto.SeatId && r.ScreeningId == dto.ScreeningId && r.Id != id);

            if (isSeatTaken)
                return BadRequest("This seat is already reserved for the selected screening.");

            reservation.FirstName = dto.FirstName;
            reservation.LastName = dto.LastName;
            reservation.Email = dto.Email;
            reservation.PhoneNumber = dto.PhoneNumber;
            reservation.SeatId = dto.SeatId;
            reservation.ScreeningId = dto.ScreeningId;

            await _context.SaveChangesAsync();

            return Ok(reservation);
        }

        // DELETE: api/Reservations/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null)
                return NotFound($"Reservation with ID {id} not found.");

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
