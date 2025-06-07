using Microsoft.AspNetCore.Mvc;
using CinemaReservationApi.Data;
using CinemaReservationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MoviesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Zwraca wszystkie filmy z bazy danych.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAll()
        {
            return await _context.Movies.ToListAsync();
        }

        /// <summary>
        /// Tworzy nowy film.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Movie>> Create(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = movie.Id }, movie);
        }

        /// <summary>
        /// Aktualizuje dane istniejącego filmu.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Movie updatedMovie)
        {
            if (id != updatedMovie.Id)
            {
                return BadRequest("ID w URL i obiekcie nie są zgodne.");
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound("Nie znaleziono filmu do edycji.");
            }

            movie.Title = updatedMovie.Title;
            movie.Description = updatedMovie.Description;
            movie.Duration = updatedMovie.Duration;
            movie.PosterUrl = updatedMovie.PosterUrl;

            await _context.SaveChangesAsync();
            return Ok(movie);
        }

        /// <summary>
        /// Usuwa film po ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound("Nie znaleziono filmu o podanym ID.");
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return Ok($"Film o ID {id} został usunięty.");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetById(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound("Nie znaleziono filmu o podanym ID.");
            }

            return Ok(movie);
        }

    }
}
