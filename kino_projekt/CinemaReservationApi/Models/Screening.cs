using CinemaReservationApi.Models;
using System.Collections.Generic;

public class Screening
{
    public int Id { get; set; }

    public int MovieId { get; set; } // FK
    public Movie? Movie { get; set; }

    public DateTime ScreeningTime { get; set; }

    public string HallNumber { get; set; } = string.Empty;

    // 🔧 DODAJ TE WŁAŚCIWOŚCI NAWIGACYJNE:
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
