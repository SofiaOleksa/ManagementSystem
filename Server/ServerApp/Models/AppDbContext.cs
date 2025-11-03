using Microsoft.EntityFrameworkCore;
using System;

namespace ServerApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Coach> Coaches { get; set; }
        public DbSet<TrainingClass> Classes { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ----------------- ВІДНОШЕННЯ -----------------

            // Class -> Coach (без каскадного видалення, щоб не було Multiple Cascade Paths)
            modelBuilder.Entity<TrainingClass>()
                .HasOne(c => c.Coach)
                .WithMany(co => co.Classes)
                .HasForeignKey(c => c.CoachId)
                .OnDelete(DeleteBehavior.Restrict);   // або .NoAction

            // Booking -> Class (можна каскад)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Class)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            // Booking -> Coach (без каскаду)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Coach)
                .WithMany(co => co.Bookings)
                .HasForeignKey(b => b.CoachId)
                .OnDelete(DeleteBehavior.Restrict);   // або .NoAction

            // ----------------- СИДІНГ ДАНИХ -----------------

            var t1 = new DateTime(2025, 11, 3, 10, 0, 0, DateTimeKind.Utc);
            var t2 = new DateTime(2025, 11, 3, 12, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Coach>().HasData(
                new Coach { Id = 1, Name = "Alice Trainer", Email = "alice@example.com", PasswordHash = "pw1" },
                new Coach { Id = 2, Name = "Bob Coach", Email = "bob@example.com", PasswordHash = "pw2" }
            );

            modelBuilder.Entity<TrainingClass>().HasData(
                new TrainingClass { Id = 1, Name = "Yoga", TimeSlot = t1, CoachId = 1 },
                new TrainingClass { Id = 2, Name = "Boxing", TimeSlot = t2, CoachId = 2 }
            );

            modelBuilder.Entity<Booking>().HasData(
                new Booking { Id = 1, CoachId = 1, ClassId = 1, ClientName = "Оксана", Status = true },
                new Booking { Id = 2, CoachId = 2, ClassId = 2, ClientName = "Назар", Status = false }
            );
        }
    }
}
