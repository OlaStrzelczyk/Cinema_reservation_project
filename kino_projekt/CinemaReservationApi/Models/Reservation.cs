using CinemaReservationApi.Models;

public class Reservation
{
    public int Id { get; set; }

    public int ScreeningId { get; set; }
    public Screening Screening { get; set; }

    public int SeatId { get; set; }
    public Seat Seat { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public DateTime ReservationTime { get; set; } = DateTime.Now;
}
