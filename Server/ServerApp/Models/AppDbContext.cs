using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ASP_proj.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ключі
            modelBuilder.Entity<Coach>().HasKey(c => c.CoachId);
            modelBuilder.Entity<Class>().HasKey(c => c.ClassId);
            modelBuilder.Entity<Booking>().HasKey(b => b.BookingId);

            // Зв'язок Coach (1) – (many) Class
            modelBuilder.Entity<Class>()
                .HasOne(c => c.Coach)
                .WithMany(co => co.Classes)
                .HasForeignKey(c => c.CoachId)
                .OnDelete(DeleteBehavior.Restrict);

            // Зв'язок Coach (1) – (many) Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Coach)
                .WithMany(co => co.Bookings)
                .HasForeignKey(b => b.CoachId)
                .OnDelete(DeleteBehavior.Restrict);

            // Зв'язок Class (1) – (many) Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Class)
                .WithMany(cl => cl.Bookings)
                .HasForeignKey(b => b.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---- Seed-дані ----

            // Тренери
            modelBuilder.Entity<Coach>().HasData(
                new Coach
                {
                    CoachId = 1,
                    Name = "Ivan Trainer",
                    Email = "ivan.trainer@example.com",
                    PasswordHash = "pass1"
                },
                new Coach
                {
                    CoachId = 2,
                    Name = "Olena Fit",
                    Email = "olena.fit@example.com",
                    PasswordHash = "pass2"
                }
            );

            // Заняття
            modelBuilder.Entity<Class>().HasData(
                new Class
                {
                    ClassId = 1,
                    Name = "Morning Cardio",
                    CoachId = 1,
                    TimeSlot = "Mon 08:00–09:00"
                },
                new Class
                {
                    ClassId = 2,
                    Name = "Evening Strength",
                    CoachId = 2,
                    TimeSlot = "Wed 18:00–19:30"
                }
            );

            // Бронювання
            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    BookingId = 1,
                    CoachId = 1,
                    ClassId = 1,
                    ClientName = "Petro Client",
                    Status = "OnTime"
                },
                new Booking
                {
                    BookingId = 2,
                    CoachId = 2,
                    ClassId = 2,
                    ClientName = "Anna Client",
                    Status = "Delayed"
                }
            );
        }
    }
}
