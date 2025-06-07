using Microsoft.EntityFrameworkCore;
using CinemaReservationApi.Models;

namespace CinemaReservationApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Screening> Screenings { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        //  Konfiguracja relacji aby uniknąć konfliktu ON DELETE CASCADE
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Seat)
                .WithMany()
                .HasForeignKey(r => r.SeatId)
                .OnDelete(DeleteBehavior.Restrict); //  blokujemy kasowanie miejsc z CASCADE
        }
    }
}
