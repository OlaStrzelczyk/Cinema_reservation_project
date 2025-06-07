using System.Text.Json.Serialization;

namespace CinemaReservationApi.Models
{
    public class Seat
    {
        public int Id { get; set; }

        public int ScreeningId { get; set; }

        [JsonIgnore] // ✅ ignoruj wymaganie tego pola przy POST/PUT
        public Screening? Screening { get; set; }

        public string SeatNumber { get; set; } = string.Empty;
    }
}
