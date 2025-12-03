using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ServerApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        // DTO для створення Booking
        public class CreateBookingRequest
        {
            [Required]
            public int CoachId { get; set; }

            [Required]
            public int ClassId { get; set; }

            [Required]
            public string ClientName { get; set; } = string.Empty;

            // Наприклад: "Pending", "Confirmed", "Cancelled"
            [Required]
            public string Status { get; set; } = "Pending";
        }

        // POST: /api/bookings
        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking([FromBody] CreateBookingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Перевіряємо, що Coach існує
            var coachExists = await _context.Coaches
                .AnyAsync(c => c.CoachId == request.CoachId);

            if (!coachExists)
            {
                return BadRequest("Coach with given CoachId does not exist.");
            }

            // Перевіряємо, що Class існує
            var classExists = await _context.Classes
                .AnyAsync(c => c.ClassId == request.ClassId);

            if (!classExists)
            {
                return BadRequest("Class with given ClassId does not exist.");
            }

            // Створюємо Booking
            var booking = new Booking
            {
                CoachId = request.CoachId,
                ClassId = request.ClassId,
                ClientName = request.ClientName,
                Status = request.Status
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetBookingById),
                new { id = booking.BookingId },
                booking);
        }

        // GET: /api/bookings/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBookingById(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Coach)
                .Include(b => b.Class)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null) return NotFound();

            return Ok(booking);
        }
    }
}
