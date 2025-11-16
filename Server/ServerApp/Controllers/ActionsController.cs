using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Controllers
{
    [ApiController]
    [Route("api/actions")]
    [Authorize(Roles = "Admin")]   // захист ендпоінта
    public class ActionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ActionsController(AppDbContext context)
        {
            _context = context;
        }

        // DTO для створення "дії" (бронювання)
        public class ActionCreateDto
        {
            public int CoachId { get; set; }
            public int ClassId { get; set; }
            public string ClientName { get; set; } = null!;
            public bool Status { get; set; }
        }

        // POST: /api/actions
        [HttpPost]
        public async Task<ActionResult<object>> CreateAction([FromBody] ActionCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // перевіряємо, чи існують такі Coach і Class
            var coachExists = await _context.Coaches.AnyAsync(c => c.Id == dto.CoachId);
            if (!coachExists)
                ModelState.AddModelError(nameof(dto.CoachId), "Вказаний тренер не існує.");

            var classExists = await _context.Classes.AnyAsync(c => c.Id == dto.ClassId);
            if (!classExists)
                ModelState.AddModelError(nameof(dto.ClassId), "Вказаний клас не існує.");

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var booking = new Booking
            {
                CoachId = dto.CoachId,
                ClassId = dto.ClassId,
                ClientName = dto.ClientName,
                Status = dto.Status
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var result = new
            {
                booking.Id,
                booking.CoachId,
                booking.ClassId,
                booking.ClientName,
                booking.Status
            };

            return Created("/api/actions/" + booking.Id, result);
        }
    }
}
