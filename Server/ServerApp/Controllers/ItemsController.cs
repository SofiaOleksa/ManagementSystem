using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Controllers
{
    [ApiController]
    [Route("api/items")]
    [Authorize(Roles = "Admin")]   // захист ендпоінта
    public class ItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetItems()
        {
            var items = await _context.Classes
                .Include(c => c.Coach)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.TimeSlot,
                    c.CoachId,
                    CoachName = c.Coach.Name
                })
                .ToListAsync();

            return Ok(items);
        }
    }
}
